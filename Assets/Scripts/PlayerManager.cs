using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerManager : MonoBehaviour
{
    public PlayerState CurrentPlayerState;
    public enum PlayerState
    {
        None,
        Idle,
        Jumping,
        Move,
    }

    internal protected Rigidbody Rb;
    internal protected bool OneTriggerJump = false;
    public float smoothTime = 0.3F;

    private Vector3 PreMousePosition;
    private Vector3 CurrentMousePosition;
    private Vector3 DeltaMousePosition;

    [Header("KAYDIRMA HIZI")]
    [SerializeField] private float SwipeSpeed;
    [Header("HARAKET HIZI")]
    [SerializeField] private float MoveSpeed;
    internal float TempMoveSpeed;

    [Header("TOPLAR ARASI MESAFE")]
    [SerializeField] float BallBetweenDistance;

    [Header("X EKSENINDE KI HAREKET SINIRLAMA")]
    [SerializeField] private float MinClampX;
    [SerializeField] private float MaxClampX;

    private int JumpPlatformListIndex = 0;

    [SerializeField] private List<GameObject> TrailObjects = new List<GameObject>();
    [SerializeField] GameObject TrailObjectPrefab;
    [SerializeField] Vector3 OffsetTrail;
    [SerializeField] float TrailLerpSpeed = 10f;

    internal Vector3 StartPosition;

    private PlayerTrigger playerTrigger;

    private void Awake()
    {
        PlayerSetUp();
    }

    private void Update()
    {
        if (TrailObjects.Count > 0)
        {
            for (int i = 0; i < TrailObjects.Count; i++)
            {
                if (i != 0)
                {
                    float TempX = Mathf.Lerp(TrailObjects[i].transform.position.x, TrailObjects[i - 1].transform.position.x, TrailLerpSpeed * Time.deltaTime);
                    float TempY = Mathf.Lerp(TrailObjects[i].transform.position.y, TrailObjects[i - 1].transform.position.y, TrailLerpSpeed * Time.deltaTime);
                    float TempZ = Mathf.Lerp(TrailObjects[i].transform.position.z, TrailObjects[i - 1].transform.position.z - BallBetweenDistance, TrailLerpSpeed * Time.deltaTime);

                    TrailObjects[i].transform.position = new Vector3(TempX, TempY, TempZ);
                }
                else
                {
                    float TempX = Mathf.Lerp(TrailObjects[0].transform.position.x, transform.position.x, TrailLerpSpeed * Time.deltaTime);
                    float TempY = Mathf.Lerp(TrailObjects[0].transform.position.y, transform.position.y, TrailLerpSpeed * Time.deltaTime);
                    float TempZ = Mathf.Lerp(TrailObjects[0].transform.position.z, transform.position.z - BallBetweenDistance, TrailLerpSpeed * Time.deltaTime);

                    TrailObjects[i].transform.position = new Vector3(TempX, TempY, TempZ);
                }
            }
        }

        if (GameManager.Instance.CurrentGameState != GameManager.GameState.Playıng)
            return;

        Move();
    }

    internal protected void Move()
    {
        if (Input.GetMouseButtonDown(0))
            CurrentMousePosition = Input.mousePosition;

        if (Input.GetMouseButton(0))
        {
            PreMousePosition = CurrentMousePosition;
            CurrentMousePosition = Input.mousePosition;
            DeltaMousePosition = ((CurrentMousePosition - PreMousePosition) / Screen.width) * SwipeSpeed;
        }

        if (Input.GetMouseButtonUp(0))
            DeltaMousePosition = Vector3.zero;

        if (CurrentPlayerState != PlayerState.Move)
            TempMoveSpeed = 0f;
        else
            TempMoveSpeed = MoveSpeed;

        transform.position = new Vector3(Mathf.Clamp(transform.position.x, MinClampX, MaxClampX), transform.position.y, transform.position.z);
        transform.position = Vector3.Slerp(transform.position, transform.position + new Vector3(DeltaMousePosition.x, 0, TempMoveSpeed), smoothTime * Time.deltaTime);
    }

    /// <summary>
    /// Playerin zıplama methodu
    /// </summary>
    internal protected void Jump()
    {
        OneTriggerJump = true;
        SetPlayerState(PlayerState.Jumping);
        Rb.isKinematic = false;
        Launch();
    }

    internal protected IEnumerator AddTrail(byte count)
    {
        for (int i = 0; i < count; i++)
        {
            int tempI = i;
            GameObject obj = Instantiate(TrailObjectPrefab, transform.position + new Vector3(0, 0, -(BallBetweenDistance + 0.5f) * ((TrailObjects.Count + tempI) + 1)), Quaternion.identity);
            TrailObjects.Add(obj);
            StartScaleAnimation(obj);
            yield return new WaitForSeconds(0.05f);
        }

        yield return null;
    }

    internal protected void RemoveTrail(bool IsSuccess)
    {
        if (TrailObjects.Count > 0)
        {
            Destroy(TrailObjects[TrailObjects.Count - 1]);
            TrailObjects.RemoveAt(TrailObjects.Count - 1);
        }
        else
        {
            if (IsSuccess)
            {
                gameObject.SetActive(false);
                GameManager.Instance.GameSuccess();
            }
            else
            {
                gameObject.SetActive(false);
                GameManager.Instance.GameFail();
            }
        }

        for (int i = 0; i < TrailObjects.Count; i++)
        {
            TrailObjects[i].transform.position = TrailObjects[i].transform.position + new Vector3(0, 0, -BallBetweenDistance);
        }
        transform.position = transform.position + new Vector3(0, 0, -BallBetweenDistance);
    }

    internal protected void StartScaleAnimation(GameObject obj)
    {
        obj.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        obj.transform.DOScale(1f, 0.1f).SetEase(Ease.Linear).OnComplete(() =>
        {
            StartCoroutine(ScaleAnimation(obj));
        });
    }

    internal protected IEnumerator ScaleAnimation(GameObject obj)
    {
        obj.transform.DOScale(1.5f, 0.5f).OnComplete(() =>
        {
            obj.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.Linear);
        }).SetEase(Ease.Linear);

        yield return null;
    }

    private void PlayerSetUp()
    {
        StartPosition = transform.position;
        Rb = GetComponent<Rigidbody>();
        Rb.isKinematic = true;
        TempMoveSpeed = MoveSpeed;
        playerTrigger = GetComponentInChildren<PlayerTrigger>();
    }

    internal protected void SetPlayerState(PlayerState playerState)
    {
        CurrentPlayerState = playerState;
    }

    private void Launch()
    {
        Transform TargetTransform = null;
        Physics.gravity = Vector3.up * gravity;
        Rb.useGravity = true;
        if ((JumpPlatformListIndex + 1) < GameManager.Instance.stageController.JumpPlatformList.Count)
        {
            JumpPlatformListIndex++;
            TargetTransform = GameManager.Instance.stageController.JumpPlatformList[JumpPlatformListIndex].transform;
        }
        else
        {
            GameObject FinalPoint = new GameObject();
            FinalPoint.transform.position = transform.position + new Vector3(0, 0, 10f);
            TargetTransform = FinalPoint.transform;
        }


        Rb.velocity = CalculateLaunchData(TargetTransform);
    }

    public float gravity = -18;
    public float h;
    private Vector3 CalculateLaunchData(Transform transform)
    {
        float displacementY = transform.position.y - Rb.position.y;
        Vector3 displacementZ = new Vector3(0f, 0f, transform.position.z - Rb.position.z);
        float time = Mathf.Sqrt(-2 * h / gravity) + Mathf.Sqrt(2 * (displacementY - h) / gravity);
        Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * gravity * h);
        Vector3 velocityZ = displacementZ / time;

        return velocityZ + velocityY * Mathf.Sign(h);
    }

    public void DefaultValues()
    {
        transform.position = StartPosition;
        TempMoveSpeed = 0f;
        Rb.isKinematic = true;
        playerTrigger.Onetime = false;
        JumpPlatformListIndex = 0;

        foreach (GameObject item in TrailObjects)
            Destroy(item);
        TrailObjects = new List<GameObject>();
    }
}
