
namespace Player
{
    public class Data
    {
        public float MoveSpeed => _moveSpeed;
        private float _moveSpeed;
        public float RotationSpeed => _rotationSpeed;
        private float _rotationSpeed;
        public float JumpForce => _jumpForce;
        private float _jumpForce;
        public const float SIT_COEFF = 0.5f;
        public const float RUN_COEFF = 3f;

        public Data(float moveSpeed, float rotationSpeed, float jumpForce)
        {
            _moveSpeed = moveSpeed;
            _rotationSpeed = rotationSpeed;
            _jumpForce = jumpForce;
        }
    }
}
