using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class ScrollObject : MonoBehaviour
{
    public float speed = 1.0f;
    public float startPosition;
    public float endPosition;

    void Update()
    {
        // 毎フレームxポジションを少しずつ移動させる
        transform.Translate(-1 * speed * Time.deltaTime, 0, 0);

        // スクロールが目標ポイントまで到達したかをチェック
        if (transform.position.x <= endPosition) ScrollEnd();
    }

    void ScrollEnd()
    {
        // 通り過ぎた分を加味してポジションを再設定
        // endPosition(-8)きっちりで実行されるわけではないので、diffがないと少し隙間が空いてしまう。
        float diff = transform.position.x - endPosition;
        Vector3 restartPostion = transform.position;
        restartPostion.x = startPosition + diff;
        transform.position = restartPostion;

        // 同じゲームオブジェクトにアタッチされているコンポーネントにメッセージを送る（他のscriptにも通知することができる）
        SendMessage("OnScrollEnd", SendMessageOptions.DontRequireReceiver);
    }
}
