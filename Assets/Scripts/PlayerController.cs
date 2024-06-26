using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    // HP
    public int hp = 5;

    // 移動中かどうか
    bool isMoving = false;
    // 1マス当たりの移動速度
    public float moveSpeed = 0.2f;
    // 移動先
    Vector3 targetDirection;

    // 向き
    public Vector3 currentDirection = Vector3.down;

    //Animator animator;

    // 当たり判定用のレイヤーを取得
    public LayerMask detectionMask;

    // Start is called before the first frame update
    void Start()
    {
        //animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // 動ける状態だったら
        if (!isMoving)
        {
            if (Input.GetKey("up"))
            {
                MoveDirection(Vector3.up, "WalkUp");
                currentDirection = Vector3.up;
            }
            else if (Input.GetKey("down"))
            {
                MoveDirection(Vector3.down, "WalkDown");
                currentDirection = Vector3.down;
            }
            else if (Input.GetKey("left"))
            {
                MoveDirection(Vector3.left, "WalkLeft");
                currentDirection = Vector3.left;
            }
            else if (Input.GetKey("right"))
            {
                MoveDirection(Vector3.right, "WalkRight");
                currentDirection = Vector3.right;
            }

            if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
            {
                Debug.Log("Attack呼び出し");
                StartCoroutine(Attack());
            }
        }
        transform.Translate(Input.GetAxis("Horizontal") * 5.0f * Time.deltaTime, 0, 0);
        transform.Translate(0, Input.GetAxis("Vertical") * 5.0f * Time.deltaTime, 0);
    }

    void MoveDirection(Vector3 direction, string animation)
    {
        //ResetAnimation();
        targetDirection = direction;
        //animator.SetBool(animation, true);

        // 進行方向に障害物がないかチェック
        if (Physics2D.Raycast(transform.position, targetDirection, 1f, detectionMask).collider != null)
        {
            Debug.Log(targetDirection + "に障害物検知");
            isMoving = false;
        }
        else // なければMove呼び出し
        {
            StartCoroutine(Move());
        }
    }

    void ResetAnimation() { }

    public IEnumerator Move()
    {
        // 移動中かどうか
        isMoving = true;
        // 現在地
        Vector3 nowPosition = transform.position;
        // 目的地
        Vector3 targetPosition = nowPosition + targetDirection;


        // 移動にかかった時間
        float elapsedTime = 0f;

        // 移動が完了するまでLerpでマス間の位置を補完
        while (elapsedTime < moveSpeed)
        {
            transform.position = Vector3.Lerp(nowPosition, targetPosition, (elapsedTime / moveSpeed));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 移動時間分経過したら目的地へ強制的に移動（処理落ち保険）
        transform.position = targetPosition;
        // 移動中フラグを戻す
        isMoving = false;

        // 移動が入力され続けてれば引き続き移動する
        if (Input.GetKey("up"))
        {
            MoveDirection(Vector3.up, "WalkUp");
            currentDirection = Vector3.up;
        }
        else if (Input.GetKey("down"))
        {
            MoveDirection(Vector3.down, "WalkDown");
            currentDirection = Vector3.down;
        }
        else if (Input.GetKey("left"))
        {
            MoveDirection(Vector3.left, "WalkLeft");
            currentDirection = Vector3.left;
        }
        else if (Input.GetKey("right"))
        {
            MoveDirection(Vector3.right, "WalkRight");
            currentDirection = Vector3.right;
        }
    }

    public ParticleSystem attackEffect;

    public IEnumerator Attack()
    {
        Debug.Log("Attackが呼び出されました");
        Collider2D hit = Physics2D.Raycast(transform.position, currentDirection, 1f, detectionMask).collider;
        if (hit != null)
        {
            if (hit.CompareTag("Enemy"))
            {
                Debug.Log(hit.gameObject.name + "に攻撃");

                // エフェクトを再生
                ParticleSystem attackParticle = Instantiate(attackEffect, transform.position + currentDirection, Quaternion.identity);
                attackParticle.Play();

                // エフェクトが再生し終わるまで待機する
                yield return new WaitForSeconds(attackParticle.main.duration);

                // エフェクトオブジェクトを削除
                Destroy(attackParticle.gameObject);
            }
            yield return null;
        }
        else
        {
            Debug.Log("攻撃対象が見つかりませんでした");
        }
    }
}
