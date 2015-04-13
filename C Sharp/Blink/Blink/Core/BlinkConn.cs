﻿using Net.Qiujuer.Blink.Async;
using Net.Qiujuer.Blink.Box;
using Net.Qiujuer.Blink.Listener;
using Net.Qiujuer.Blink.Listener.Delivery;
using Net.Qiujuer.Blink.Tool;
using System;
using System.IO;

namespace Net.Qiujuer.Blink.Core
{
    public class BlinkConn : IDestroy
    {
        private readonly ISender mSender;

        private readonly ISendDelivery mSendDelivery;

        private readonly IReceiver mReceiver;

        private readonly IReceiveDelivery mReceiveDelivery;

        private readonly IResource mResource;

        private readonly IBlinkParser mParser;

        private SendDispatcher mSendDispatcher;
        private ReceiveDispatcher mReceiveDispatcher;


        public BlinkConn(ISender sender, ISendDelivery sendDelivery, IReceiver receiver, IReceiveDelivery receiveDelivery, IResource resource, IBlinkParser parser)
        {
            mSender = sender;
            mReceiver = receiver;
            mResource = resource;

            mSendDelivery = sendDelivery;
            mReceiveDelivery = receiveDelivery;

            mParser = parser;

            // Init this
            Init();
        }

        /**
         * Starts the dispatchers in this queue.
         */
        private void Init()
        {
            // Create the cache dispatcher and start it.
            mSendDispatcher = new SendDispatcher(mSender, mSendDelivery);

            mReceiveDispatcher = new ReceiveDispatcher(mReceiver, mParser, mReceiveDelivery);
        }

        /**
         * Stops the cache and network dispatchers.
         */
        public void Destroy()
        {
            if (mSendDelivery != null)
                mSendDelivery.Destroy();

            if (mReceiveDelivery != null)
                mReceiveDelivery.Destroy();

            if (mResource != null)
                mResource.Clear();

            if (mSender != null)
                mSender.Destroy();

            if (mReceiver != null)
                mReceiver.Destroy();

            if (mSendDispatcher != null)
                mSendDispatcher.Destroy();

            if (mReceiveDispatcher != null)
                mReceiveDispatcher.Destroy();

        }

        /**
         * Get file resource
         *
         * @return Resource
         */
        public IResource GetResource()
        {
            return mResource;
        }

        /**
         * Send a Entity to queue
         *
         * @param entity SendEntity<T> {@link SendPacket}
         * @param <T>    Extends SendEntity
         * @return SendEntity<T>
         */
        public SendPacket Send(SendPacket entity)
        {
            entity.SetBlinkConn(this);

            mSendDispatcher.Send(entity);

            return entity;
        }

        /**
         * Send file to queue
         *
         * @param file File
         * @return FileSendEntity {@link FileSendPacket}
         */
        public FileSendPacket Send(FileInfo file)
        {
            return Send(file, null);
        }

        /**
         * Send file to queue
         *
         * @param file     File
         * @param listener Callback listener
         * @return FileSendEntity {@link FileSendPacket}
         */
        public FileSendPacket Send(FileInfo file, SendListener listener)
        {
            FileSendPacket entity = new FileSendPacket(file, listener);
            Send(entity);
            return entity;
        }

        /**
         * Send byte array to queue
         *
         * @param bytes Byte array
         * @return ByteSendEntity {@link ByteSendPacket}
         */
        public ByteSendPacket Send(byte[] bytes)
        {
            return Send(bytes, null);
        }

        /**
         * Send byte array to queue
         *
         * @param bytes    Byte array
         * @param listener Callback listener
         * @return ByteSendEntity {@link ByteSendPacket}
         */
        public ByteSendPacket Send(byte[] bytes, SendListener listener)
        {
            ByteSendPacket entity = new ByteSendPacket(bytes, listener);
            Send(entity);
            return entity;
        }

        /**
         * Send string to queue
         *
         * @param str String msg
         * @return StringSendEntity {@link StringSendPacket}
         */
        public StringSendPacket Send(String str)
        {
            return Send(str, null);
        }

        /**
         * Send string to queue
         *
         * @param str      String msg
         * @param listener Callback listener
         * @return StringSendEntity {@link StringSendPacket}
         */
        public StringSendPacket Send(String str, SendListener listener)
        {
            StringSendPacket entity = null;
            try
            {
                entity = new StringSendPacket(str, listener);
                Send(entity);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return entity;
        }
        internal void Cancel(SendPacket packet)
        {
            //mSendQueue.(entity);
        }
    }
}