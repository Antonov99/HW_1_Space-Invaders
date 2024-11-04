using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Inventories
{
    public sealed class Inventory : IEnumerable<Item>
    {
        public event Action<Item, Vector2Int> OnAdded;
        public event Action<Item, Vector2Int> OnRemoved;
        public event Action<Item, Vector2Int> OnMoved;
        public event Action OnCleared;

        public int Width { get; private set; }
        public int Height { get; private set; }
        public int Count { get; private set; }

        public readonly Item[,] cells;
        public readonly Dictionary<Item, List<Vector2Int>> itemMap;

        public Inventory(in int width, in int height)
        {
            if (width < 0 || height < 0 || (width == 0 && height == 0)) throw new ArgumentException();
            Width = width;
            Height = height;
            cells = new Item[width, height];
            itemMap = new Dictionary<Item, List<Vector2Int>>();
        }

        public Inventory(
            in int width,
            in int height,
            params KeyValuePair<Item, Vector2Int>[] items
        ) : this(width, height)
        {
            if (items is null) throw new ArgumentException();
            foreach (var item in items)
            {
                AddItem(item.Key, item.Value);
            }
        }

        public Inventory(
            in int width,
            in int height,
            params Item[] items
        ) : this(width, height)
        {
            if (items is null) throw new ArgumentException();
            foreach (var item in items)
            {
                AddItem(item);
            }
        }

        public Inventory(
            in int width,
            in int height,
            in IEnumerable<KeyValuePair<Item, Vector2Int>> items
        ) : this(width, height)
        {
            if (items is null) throw new ArgumentException();
            foreach (var item in items)
            {
                AddItem(item.Key, item.Value);
            }
        }

        public Inventory(
            in int width,
            in int height,
            in IEnumerable<Item> items
        ) : this(width, height)
        {
            if (items is null) throw new ArgumentException();
            foreach (var item in items)
            {
                AddItem(item);
            }
        }

        /// <summary>
        /// Checks for adding an item on a specified position
        /// </summary>
        public bool CanAddItem(in Item item, in Vector2Int position)
        {
            if (item is null) return false;
            if (item.Size.x <= 0 || item.Size.y <= 0) throw new ArgumentException();
            if (Contains(item)) return false;

            Vector2Int itemSize = item.Size;

            if (position.x + itemSize.x > Width ||
                position.y + itemSize.y > Height ||
                position.x < 0 || position.y < 0)
            {
                return false;
            }

            for (int x = position.x; x < position.x + itemSize.x; x++)
            {
                for (int y = position.y; y < itemSize.y + position.y; y++)
                {
                    if (cells[x, y] != null)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public bool CanAddItem(in Item item, in int posX, in int posY)
        {
            if (item is null) return false;
            if (item.Size.x <= 0 || item.Size.y <= 0) throw new ArgumentException();
            if (Contains(item)) return false;

            Vector2Int itemSize = item.Size;

            if (posX + itemSize.x > Width ||
                posY + itemSize.y > Height ||
                posX < 0 || posY < 0)
            {
                return false;
            }

            for (int x = posX; x < posX + itemSize.x; x++)
            {
                for (int y = posY; y < itemSize.y + posY; y++)
                {
                    if (cells[x, y] != null)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Adds an item on a specified position if not exists
        /// </summary>
        public bool AddItem(in Item item, in Vector2Int position)
        {
            if (!CanAddItem(item, position))
            {
                return false;
            }

            Vector2Int itemSize = item.Size;
            List<Vector2Int> points = new List<Vector2Int>(itemSize.x * itemSize.y);

            for (int x = position.x; x < position.x + itemSize.x; x++)
            {
                for (int y = position.y; y < itemSize.y + position.y; y++)
                {
                    cells[x, y] = item;
                    points.Add(new Vector2Int(x, y));
                }
            }

            itemMap.Add(item, points);
            OnAdded?.Invoke(item, position);
            Count++;

            return true;
        }

        public bool AddItem(in Item item, in int posX, in int posY)
        {
            if (!CanAddItem(item, posX, posY))
            {
                return false;
            }

            Vector2Int itemSize = item.Size;
            List<Vector2Int> points = new List<Vector2Int>(itemSize.x * itemSize.y);

            for (int x = posX; x < posX + itemSize.x; x++)
            {
                for (int y = posY; y < itemSize.y + posY; y++)
                {
                    cells[x, y] = item;
                    points.Add(new Vector2Int(x, y));
                }
            }

            itemMap.Add(item, points);
            OnAdded?.Invoke(item, new Vector2Int(posX, posY));
            Count++;

            return true;
        }

        /// <summary>
        /// Checks for adding an item on a free position
        /// </summary>
        public bool CanAddItem(in Item item)
        {
            if (item is null) return false;
            if (item.Size.x <= 0 || item.Size.y <= 0) throw new ArgumentException();
            if (Contains(item)) return false;
            if (!FindFreePosition(item, out Vector2Int freePosition)) return false;
            return true;
        }

        /// <summary>
        /// Adds an item on a free position
        /// </summary>
        public bool AddItem(in Item item)
        {
            if (!CanAddItem(item)) return false;
            if (!FindFreePosition(item, out Vector2Int freePosition)) return false;

            Vector2Int itemSize = item.Size;
            List<Vector2Int> points = new List<Vector2Int>(itemSize.x * itemSize.y);

            for (int x = freePosition.x; x < freePosition.x + itemSize.x; x++)
            {
                for (int y = freePosition.y; y < freePosition.y + itemSize.y; y++)
                {
                    cells[x, y] = item;
                    points.Add(new Vector2Int(x, y));
                }
            }

            itemMap.Add(item, points);
            OnAdded?.Invoke(item, freePosition);
            Count++;
            return true;
        }

        /// <summary>
        /// Returns a free position for a specified item
        /// </summary>
        public bool FindFreePosition(in Item item, out Vector2Int freePosition)
        {
            freePosition = default;
            var size = item.Size;
            for (int x = 0; x <= Width - size.x; x++)
            {
                for (int y = 0; y <= Height - size.y; y++)
                {
                    bool free = true;
                    for (int i = x; i < x + size.x; i++)
                    {
                        for (int j = y; j < y + size.y; j++)
                        {
                            if (cells[i, j] == null) continue;
                            free = false;
                            break;
                        }

                        if (!free) break;
                    }

                    if (!free) continue;
                    freePosition = new Vector2Int(x, y);
                    return true;
                }
            }

            return false;
        }

        public bool FindFreePosition(in Vector2Int size, out Vector2Int freePosition)
        {
            freePosition = default;
            if (size.x <= 0 || size.y <= 0) throw new ArgumentException();

            for (int y = 0; y <= Height - size.y; y++) 
            {
                for (int x = 0; x <= Width - size.x; x++)
                {
                    bool free = true;
                    for (int i = x; i < x + size.x; i++)
                    {
                        for (int j = y; j < y + size.y; j++)
                        {
                            if (cells[i, j] != null)
                            {
                                free = false;
                                break; 
                            }
                        }
                        if (!free) break; 
                    }

                    if (free)
                    {
                        freePosition = new Vector2Int(x, y);
                        return true;
                    }
                }
            }

            return false;
        }

        public bool FindFreePosition(in int sizeX, int sizeY, out Vector2Int freePosition)
        {
            freePosition = default;
            if (sizeX <= 0 || sizeY <= 0) throw new ArgumentException();
            for (int x = 0; x <= Width - sizeX; x++)
            {
                for (int y = 0; y <= Height - sizeY; y++)
                {
                    bool free = true;
                    for (int i = x; i < x + sizeX; i++)
                    {
                        for (int j = y; j < y + sizeY; j++)
                        {
                            if (cells[i, j] == null) continue;
                            free = false;
                            break;
                        }

                        if (!free) break;
                    }

                    if (!free) continue;
                    freePosition = new Vector2Int(x, y);
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Checks if a specified item exists
        /// </summary>
        public bool Contains(in Item item)
        {
            if (item is null) return false;
            return itemMap.ContainsKey(item);
        }

        /// <summary>
        /// Checks if a specified position is occupied
        /// </summary>
        public bool IsOccupied(in Vector2Int position)
        {
            return cells[position.x, position.y] != null;
        }

        public bool IsOccupied(in int x, in int y)
        {
            return cells[x, y] != null;
        }

        /// <summary>
        /// Checks if the a position is free
        /// </summary>
        public bool IsFree(in Vector2Int position)
        {
            return cells[position.x, position.y] == null;
        }

        public bool IsFree(in int x, in int y)
        {
            return cells[x, y] == null;
        }

        /// <summary>
        /// Removes a specified item if exists
        /// </summary>
        public bool RemoveItem(in Item item)
        {
            if (item is null) return false;
            if (!itemMap.ContainsKey(item)) return false;

            var position = itemMap[item];
            foreach (var pos in position)
            {
                cells[pos.x, pos.y] = null;
            }

            itemMap.Remove(item);
            OnRemoved?.Invoke(item, position[0]);
            Count--;

            return true;
        }

        public bool RemoveItem(in Item item, out Vector2Int position)
        {
            position = default;
            if (item is null) return false;
            if (!Contains(item)) return false;

            var positions = itemMap[item];
            position = positions[0];
            foreach (var pos in positions)
            {
                cells[pos.x, pos.y] = null;
            }

            itemMap.Remove(item);
            OnRemoved?.Invoke(item, position);
            Count--;

            return true;
        }

        /// <summary>
        /// Returns an item at specified position 
        /// </summary>
        public Item GetItem(in Vector2Int position)
        {
            var cell = cells[position.x, position.y];
            if (cell is null) throw new NullReferenceException();
            return cell;
        }

        public Item GetItem(in int x, in int y)
        {
            var cell = cells[x, y];
            if (cell is null) throw new NullReferenceException();
            return cell;
        }

        public bool TryGetItem(in Vector2Int position, out Item item)
        {
            item = default;
            if (position.x >= Width || position.y >= Height || position.x < 0 || position.y < 0) return false;
            item = cells[position.x, position.y];
            return item != null;
        }

        public bool TryGetItem(in int x, in int y, out Item item)
        {
            item = default;
            if (x >= Width || y >= Height || x < 0 || y < 0) return false;
            item = cells[x, y];
            return item != null;
        }

        /// <summary>
        /// Returns matrix positions of a specified item 
        /// </summary>
        public Vector2Int[] GetPositions(in Item item)
        {
            if (item is null) throw new NullReferenceException();
            if (!Contains(item)) throw new KeyNotFoundException();
            return itemMap[item].ToArray();
        }

        public bool TryGetPositions(in Item item, out Vector2Int[] positions)
        {
            positions = default;
            if (!Contains(item)) return false;
            positions = itemMap[item].ToArray();
            return true;
        }

        /// <summary>
        /// Clears all inventory items
        /// </summary>
        public void Clear()
        {
            if (Count==0) return;
            var items = itemMap.Keys.ToArray();
            foreach (var item in items)
            {
                RemoveItem(item);
            }

            OnCleared?.Invoke();
        }

        /// <summary>
        /// Returns a count of items with a specified name
        /// </summary>
        public int GetItemCount(string name)
        {
            return itemMap.Keys.Count(item => item.Name == name);
        }

        /// <summary>
        /// Moves a specified item at target position if exists
        /// </summary>
        public bool MoveItem(in Item item, in Vector2Int position)
        {
            if (item is null) throw new ArgumentNullException();
            if (!Contains(item)) return false;
            if (position.x > Width || position.y > Height || position.x <= 0 || position.y <= 0) return false;
            if (!CanMoveItem(item, position)) return false;
            var positions = itemMap[item];
            foreach (var coordinates in positions)
            {
                cells[coordinates.x, coordinates.y] = null;
            }
            itemMap.Remove(item);

            Vector2Int itemSize = item.Size;
            List<Vector2Int> points = new List<Vector2Int>(itemSize.x * itemSize.y);

            for (int x = position.x; x < position.x + itemSize.x; x++)
            {
                for (int y = position.y; y < itemSize.y + position.y; y++)
                {
                    cells[x, y] = item;
                    points.Add(new Vector2Int(x, y));
                }
            }
            itemMap.Add(item, points);
            
            OnMoved?.Invoke(item,position);

            return true;
        }

        private bool CanMoveItem(in Item item, in Vector2Int position)
        {
            if (item.Size.x <= 0 || item.Size.y <= 0) throw new ArgumentException();

            Vector2Int itemSize = item.Size;

            if (position.x + itemSize.x > Width ||
                position.y + itemSize.y > Height ||
                position.x < 0 || position.y < 0)
            {
                return false;
            }

            for (int x = position.x; x < position.x + itemSize.x; x++)
            {
                for (int y = position.y; y < itemSize.y + position.y; y++)
                {
                    if (cells[x, y] != null && !cells[x,y].Equals(item))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Reorganizes a inventory space so that the free area is uniform
        /// </summary>
        public void ReorganizeSpace()
        {
            var items = itemMap.ToList();
            Clear();

            items.Sort((a, b) => (b.Value.Count).CompareTo(a.Value.Count));


            foreach (var itemPair in items)
            {
                var item = itemPair.Key;
                var size = item.Size;

                Vector2Int position;
                if (FindLowestFreePosition(size, out position))
                {
                    AddItem(item, position);
                }
                else
                {
                    Debug.LogWarning($"Not enough space for item {item.Name}");
                }

            }
        }

        private bool FindLowestFreePosition(Vector2Int size, out Vector2Int freePosition)
        {
            freePosition = default;
            for (int y = 0; y <= Height - size.y; y++)
            {
                for (int x = 0; x <= Width - size.x; x++)
                {
                    bool free = true;
                    for (int i = x; i < x + size.x; i++)
                    {
                        for (int j = y; j < y + size.y; j++)
                        {
                            if (cells[i, j] != null)
                            {
                                free = false;
                                break;
                            }
                        }
                        if (!free) break;
                    }
                    if (free)
                    {
                        freePosition = new Vector2Int(x, y);
                        return true;
                    }
                }
            }
            return false;
        }


        /// <summary>
        /// Copies inventory items to a specified matrix
        /// </summary>
        public void CopyTo(in Item[,] matrix)
        {
            if (matrix.GetLength(0) != Width || matrix.GetLength(1) != Height)
            {
                throw new ArgumentException();
            }

            Array.Copy(cells, matrix, cells.Length);
        }

        public IEnumerator<Item> GetEnumerator()
        {
            return itemMap.Keys.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}