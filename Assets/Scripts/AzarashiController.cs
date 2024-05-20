using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Callbacks;
using UnityEngine;

public class AzarashiController : MonoBehaviour
{
    Rigidbody2D rd2d;
    Animator animator;
    float angle;
    bool isDead;

    public float maxHeight;
    public float flapVelocity;
    public float relativeVelocityX;
    public GameObject sprite;

    // 死亡状態を返す
    public bool IsDead()
    {
        return isDead;
    }

    // AwakeはStartより前に実行される
    void Awake()
    {
        rd2d = GetComponent<Rigidbody2D>();
        animator = sprite.GetComponent<Animator>();
    }

    void Update()
    {
        // 最高高度に達していない場合に限りタップの入力を受け付ける
        if (Input.GetButtonDown("Fire1") && transform.position.y < maxHeight)
        {
            Flap();
        }

        // 角度を調整
        ApplyAngle();

        // angleが水平以上だったら、アニメーターのflapフラグをtrueにする
        animator.SetBool("flap", angle >= 1.0f && !isDead);
    }

    void ApplyAngle()
    {
        float targetAngle;

        // 死亡したらひっくりかえる
        if (isDead)
        {
            targetAngle = 180.0f;
        }
        else
        {
            // 現在の速度、相対速度（地面のgroundが3で移動しているのでrelativeVelocityXは3）から進んでいる角度を求める
            // Atan2は点（x, y）が原点（0, 0）から見たときの角度を返す。返される角度はラジアンで、範囲は-πからπ（-180度から180度）
            // Mathf.Rad2Degでラジアンを角度に変換する
            targetAngle = Mathf.Atan2(rd2d.velocity.y, relativeVelocityX) * Mathf.Rad2Deg;
        }
        // 回転アニメをスムージング
        angle = Mathf.Lerp(angle, targetAngle, Time.deltaTime * 10.0f);

        // Rotationの反映
        sprite.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, angle);
    }

    // 衝突イベントによる死亡判定
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDead) return;

        // 何かにぶつかったら死亡フラグをたてる
        isDead = true;
    }

    public void Flap()
    {
        // 死んだらはばだかない
        if (isDead) return;

        // 重力が効ていないときは操作しない
        if (rd2d.isKinematic) return;

        // Velocityを直接書き換えて上方向に加速
        rd2d.velocity = new Vector2(0.0f, flapVelocity);
    }

    public void SetStreerActive(bool active)
    {
        // Rididbodyのオン、オフを切り替える
        rd2d.isKinematic = !active;
    }
}
