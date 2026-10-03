using UnityEngine;

namespace Game.Scripts.Pawns
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PawnMovement : MonoBehaviour
    {
        [SerializeField] private float m_speedMax = 2f;
        
        private float m_move;
        private Rigidbody2D m_body;

        public Rigidbody2D GetBody()
        {
            return m_body;
        }

        public float GetMove()
        {
            return m_move;
        }
        
        public float GetSpeedMax()
        {
            return m_speedMax;
        }
        
        public void Move(float move)
        {
            m_move = Mathf.Clamp(move, -1f, 1f);
        }
        
        private void SetupBody()
        {
            m_body = GetComponent<Rigidbody2D>();

            m_body.constraints = RigidbodyConstraints2D.FreezeRotation;
        }

        private void UpdateBody(float deltaTime)
        {
            var body = GetBody();

            body.linearVelocityX = GetMove() * GetSpeedMax();
        }
        
        private void Awake()
        {
            SetupBody();
        }
        
        private void FixedUpdate()
        {
            UpdateBody(Time.fixedDeltaTime);
        }
    }
}
