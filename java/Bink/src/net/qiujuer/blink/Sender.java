package net.qiujuer.blink;

/**
 * Send entity interface
 */
public interface Sender {

    /**
     * Send the entity's information
     *
     * @param entity SendEntity
     * @return Status
     */
    boolean sendHead(SendEntity entity);

    /**
     * Send entity
     *
     * @param entity SendEntity
     * @return Status
     */
    boolean sendEntity(SendEntity entity, SendDelivery delivery);

    /**
     * Destroy the sender IO stream
     */
    void destroySendIO();
}
