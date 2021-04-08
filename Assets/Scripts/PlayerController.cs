using System.Collections;
using UnityEngine;

// TODO: Fix Hero positionZ change.
public class PlayerController : MonoBehaviour
{
    private Vector3 startPosition;
    private Quaternion startRotation;
    private float laneOffset = 2.5f;
    private float laneChangeSpeed = 15;
    private Rigidbody rb;
    private float startPoint;
    private float finishPoint;
    private bool isMoving = false;
    private Coroutine movingCoroutine;
    private float lastVectorX;
    private bool isJumping = false;
    private float jumpPower = 15;
    private float jumpGravity = -40;
    float realGravity = -9.8f;

    #region --- Unity methods ---

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        startPosition = transform.position;
        startRotation = transform.rotation;
        SwipeManager.instance.MoveEvent += MovePlayer;
        KeyboardManager.instance.MoveEvent += MovePlayer;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Ramp")
        {
            // Объединение заморозки вращения и позиции по Z.
            rb.constraints |= RigidbodyConstraints.FreezePositionZ;
        }
        if (other.gameObject.tag == "Lose")
        {
            ResetGame();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Ramp")
        {
            // Отмена заморозки позиции по Z.
            rb.constraints &= ~RigidbodyConstraints.FreezePositionZ;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        }
        if (collision.gameObject.tag == "NotLose")
        {
            MoveHorizontal(-lastVectorX);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "RampPlane" && isJumping == false)
        {
            if (rb.velocity.x == 0)
            {
                rb.velocity = new Vector3(rb.velocity.x, -10, rb.velocity.z);
            }
        }
    }

    #endregion

    #region --- Public methods ---

    public void StartGame()
    {
        RoadGenerator.instance.StartLevel();
    }

    public void ResetGame()
    {
        rb.velocity = Vector3.zero;
        startPoint = 0;
        finishPoint = 0;
        transform.position = startPosition;
        transform.rotation = startRotation;
        RoadGenerator.instance.ResetLevel();
    }

    #endregion

    #region --- Private Methods ---

    private void MovePlayer(bool[] moves)
    {
        if (moves[(int)Direction.Left] && finishPoint > -laneOffset)
        {
            MoveHorizontal(-laneChangeSpeed);
        }
        if (moves[(int)Direction.Right] && finishPoint < laneOffset)
        {
            MoveHorizontal(laneChangeSpeed);
        }
        if (moves[(int)Direction.Up] && isJumping == false)
        {
            Jump();
        }
    }

    private void MoveHorizontal(float speed)
    {
        startPoint = finishPoint;
        finishPoint += Mathf.Sign(speed) * laneOffset;

        if (isMoving)
        {
            StopCoroutine(movingCoroutine);
            isMoving = false;
        }

        movingCoroutine = StartCoroutine(MoveCoroutine(speed));

    }

    private void Jump()
    {
        isJumping = true;
        rb.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
        Physics.gravity = new Vector3(0, jumpGravity, 0);
        StartCoroutine(StopJumpCoroutine());
    }

    #endregion

    #region --- Coroutines ---

    private IEnumerator StopJumpCoroutine()
    {
        do
        {
            yield return new WaitForFixedUpdate();
        } while (rb.velocity.y != 0);
        isJumping = false;
        Physics.gravity = new Vector3(0, realGravity, 0);
    }

    private IEnumerator MoveCoroutine(float vectorX)
    {
        isMoving = true;

        while (Mathf.Abs(startPoint - transform.position.x) < laneOffset) // Сравнение текущего смещения героя от стартовой точки и смещения линии.
        {
            yield return new WaitForFixedUpdate();

            rb.velocity = new Vector3(vectorX, rb.velocity.y, 0); // Изменение скорости движения физического тела (героя).
            lastVectorX = vectorX;
            float x = Mathf.Clamp(transform.position.x, Mathf.Min(startPoint, finishPoint), Mathf.Max(startPoint, finishPoint));
            transform.position = new Vector3(x, transform.position.y, transform.position.z); ;
        }

        rb.velocity = Vector3.zero;
        transform.position = new Vector3(finishPoint, transform.position.y, transform.position.z);

        if (transform.position.y > 1)
        {
            rb.velocity = new Vector3(rb.velocity.x, -10, rb.velocity.z);
        }
        isMoving = false;
    }

    #endregion

}
