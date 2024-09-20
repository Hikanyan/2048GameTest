using UnityEngine;
using TMPro;
using DG.Tweening;

namespace HikanyanLaboratory.InGame
{
    public class Tile : MonoBehaviour
    {
        private int _value; // タイルの数値
        [SerializeField] private TextMeshProUGUI _valueText;

        /// <summary>
        /// タイルの値を設定し、表示を更新
        /// </summary>
        public void SetValue(int newValue)
        {
            _value = newValue;
            UpdateDisplay();
        }

        /// <summary>
        /// タイルの表示を更新
        /// </summary>
        public void UpdateDisplay()
        {
            if (_valueText != null)
            {
                _valueText.text = _value.ToString(); // タイルの値をUIに表示
            }
            else
            {
                Debug.LogWarning("TextMeshProコンポーネントが設定されていません");
            }
        }

        /// <summary>
        /// タイルの合体処理
        /// </summary>
        public void Merge(Tile otherTile)
        {
            if (otherTile == null || otherTile._value != _value) return;

            // 値を2倍にする
            _value *= 2;

            // 合体するタイルを削除
            Destroy(otherTile.gameObject);

            // 表示を更新
            UpdateDisplay();

            // 合体時のアニメーションやエフェクトをここに追加
            PlayMergeEffect();
        }

        /// <summary>
        /// 合体時のエフェクトを再生
        /// </summary>
        private void PlayMergeEffect()
        {
            // サイズを少し大きくして元に戻すアニメーション
            transform.DOScale(Vector3.one * 1.2f, 0.2f).SetEase(Ease.OutBack).OnComplete(() =>
            {
                transform.DOScale(Vector3.one, 0.2f).SetEase(Ease.InBack);
            });
        }

        /// <summary>
        /// 現在のタイルの値を取得
        /// </summary>
        public int GetValue()
        {
            return _value;
        }
    }
}