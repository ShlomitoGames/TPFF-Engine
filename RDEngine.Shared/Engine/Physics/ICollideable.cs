namespace RDEngine.Engine.Physics
{
    public interface ICollideable
    {
        void OnCollisionEnter(Collision collision)
        {

        }
        void OnCollisionStay(Collision collision)
        {

        }
        void OnCollisionExit(Collision collision)
        {

        }

        void OnTriggerEnter(RigidBody intrRb)
        {

        }
        void OnTriggerStay(RigidBody intrRb)
        {

        }
        void OnTriggerExit(RigidBody intrRb)
        {

        }
    }
}