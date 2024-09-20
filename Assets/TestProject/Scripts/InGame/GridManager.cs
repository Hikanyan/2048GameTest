using UnityEngine;
using UnityEngine.Tilemaps;

namespace HikanyanLaboratory.InGame
{
    public class GridManager : MonoBehaviour
    {
        [SerializeField] private int _gridSize = 4; // グリッドサイズ
        [SerializeField] private Tilemap _tilemap; // Tilemapを参照
        [SerializeField] private TileBase _tilePrefab; // タイルのPrefab (TileBaseを使用)
        [SerializeField] private GameObject _tileObjectPrefab; // タイルの表示を管理するオブジェクトプレハブ

        private Tile[,] _gridArray; // 実際に格納するタイルオブジェクト

        public int GridSize => _gridSize;

        /// <summary>
        /// グリッドの初期化
        /// </summary>
        public void InitializeGrid()
        {
            _gridArray = new Tile[_gridSize, _gridSize];

            // グリッドをクリアする処理を追加（Tilemapを初期化）
            _tilemap.ClearAllTiles();
        }

        /// <summary>
        /// 指定位置にタイルを生成
        /// </summary>
        public void SpawnTile(int x, int y, int value)
        {
            if (_gridArray[x, y] == null)
            {
                // タイルオブジェクトを生成して値を設定
                GameObject newTileObject = Instantiate(_tileObjectPrefab, _tilemap.transform);
                Tile newTile = newTileObject.GetComponent<Tile>();

                newTile.SetValue(value); // タイルの値を設定

                // タイルをグリッド配列に登録
                _gridArray[x, y] = newTile;

                // タイルをTilemapに反映（タイルのビジュアル管理）
                Vector3Int tilePosition = new Vector3Int(x, y, 0);
                _tilemap.SetTile(tilePosition, _tilePrefab);

                // タイルオブジェクトの位置をTilemapのグリッドに合わせる
                newTileObject.transform.position = _tilemap.GetCellCenterWorld(tilePosition);
            }
        }

        /// <summary>
        /// 指定位置にあるタイルを取得
        /// </summary>
        public Tile GetTileAt(int x, int y)
        {
            if (x >= 0 && x < _gridSize && y >= 0 && y < _gridSize)
            {
                return _gridArray[x, y];
            }
            return null; // 範囲外の場合はnullを返す
        }

        /// <summary>
        /// タイルを移動
        /// </summary>
        public void MoveTile(Vector2 direction)
        {
            for (int x = 0; x < _gridSize; x++)
            {
                for (int y = 0; y < _gridSize; y++)
                {
                    Tile currentTile = _gridArray[x, y];

                    if (currentTile != null)
                    {
                        Vector3Int newPosition = CalculateNewPosition(x, y, direction);

                        if (IsValidPosition(newPosition))
                        {
                            // タイルを新しい位置に移動
                            MoveTileToPosition(currentTile, newPosition);

                            // グリッド上でのタイル情報を更新
                            _gridArray[newPosition.x, newPosition.y] = currentTile;
                            _gridArray[x, y] = null; // 元の位置をクリア
                        }
                    }
                }
            }
        }

        /// <summary>
        /// タイルを新しい位置に移動
        /// </summary>
        private void MoveTileToPosition(Tile tile, Vector3Int newPosition)
        {
            // タイルのゲームオブジェクトの位置を移動
            tile.transform.position = _tilemap.GetCellCenterWorld(newPosition);

            // タイルをTilemap上で移動
            _tilemap.SetTile(new Vector3Int((int)tile.transform.position.x, (int)tile.transform.position.y, 0), null); // 現在位置をクリア
            _tilemap.SetTile(newPosition, _tilePrefab); // 新しい位置に移動
        }

        /// <summary>
        /// 新しいタイルの位置を計算する
        /// </summary>
        private Vector3Int CalculateNewPosition(int x, int y, Vector2 direction)
        {
            return new Vector3Int(x + (int)direction.x, y + (int)direction.y, 0);
        }

        /// <summary>
        /// 位置がグリッド内かどうかを確認
        /// </summary>
        private bool IsValidPosition(Vector3Int position)
        {
            return position.x >= 0 && position.x < _gridSize && position.y >= 0 && position.y < _gridSize;
        }

        /// <summary>
        /// グリッドが満杯かどうかを確認
        /// </summary>
        public bool IsFull()
        {
            for (int x = 0; x < _gridSize; x++)
            {
                for (int y = 0; y < _gridSize; y++)
                {
                    if (_gridArray[x, y] == null)
                    {
                        return false; // 空きがあれば満杯ではない
                    }
                }
            }
            return true; // 空きがない場合は満杯
        }
    }
}
