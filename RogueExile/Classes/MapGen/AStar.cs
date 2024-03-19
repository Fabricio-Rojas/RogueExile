using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Xml.Linq;

namespace RogueExile.Classes.MapGen
{
    internal static class AStar
    {
        public static List<Cell> FindPath(Cell[,] grid, Room startingRoom, Room targetRoom)
        {
            Cell startCell = startingRoom.RoomCenter;
            Cell targetCell = targetRoom.RoomCenter;

            // Check if the cell coordinates are within the grid boundaries
            bool IsValidCell(int x, int y)
            {
                return x >= 0 && x < grid.GetLength(0) && y >= 0 && y < grid.GetLength(1);
            }

            // Initialize the open and closed lists
            PriorityQueue<Node> openList = new PriorityQueue<Node>();
            HashSet<Cell> closedList = new HashSet<Cell>();

            // Add the starting node to the open list
            openList.Enqueue(new Node(startCell, null, 0, CalculateManhattanDistance(startCell, targetCell)));

            while (openList.Count > 0)
            {
                // Get the node with the lowest F value from the open list
                Node currentNode = openList.Dequeue();
                Cell currentCell = currentNode.Cell;

                // Check if the goal cell is reached
                if (currentCell == targetCell)
                {
                    // Reconstruct and return the path
                    List<Cell> path = new List<Cell>();
                    Node node = currentNode;
                    while (node != null)
                    {
                        path.Add(node.Cell);
                        node = node.Parent;
                    }
                    path.Reverse();
                    return path;
                }

                // Mark the current cell as visited
                closedList.Add(currentCell);

                // Generate neighboring cells
                int[] dx = { 0, 1, 0, -1 };
                int[] dy = { 1, 0, -1, 0 };

                for (int i = 0; i < 4; i++)
                {
                    int newX = currentCell.X + dx[i];
                    int newY = currentCell.Y + dy[i];

                    if (!IsValidCell(newX, newY))
                        continue;

                    Cell neighborCell = grid[newX, newY];

                    // Skip occupied or inaccessible cells
                    if (neighborCell.IsOccupied
                        && !CheckIfIn2DArray(startingRoom.RoomGrid, neighborCell)
                        && !CheckIfIn2DArray(targetRoom.RoomGrid, neighborCell)
                        && neighborCell.Val != '░'
                        || closedList.Contains(neighborCell))
                        continue;

                    int gCost = currentNode.G + 1; // Assuming each step costs 1

                    if (!openList.Any(n => n.Cell == neighborCell) || gCost < currentNode.G) // change to evaluate the neighbour node
                    {
                        Node newNode = new Node(neighborCell, currentNode, gCost, CalculateManhattanDistance(neighborCell, targetCell));

                        openList.Enqueue(newNode);
                    }
                }
            }
            // Path not found
            return null;
        }
        public static void FindLongestPath(MapGenerator map)
        {
            List<Room> rooms = map.rooms;
            List<Cell> longestPath = new List<Cell>();

            while (rooms.Count > 1)
            {
                Room currentRoom = rooms[0];
                rooms.Remove(currentRoom);

                for (int i = 0; i < rooms.Count; i++)
                {
                    Room comparedRoom = rooms[i];

                    Cell startCell = currentRoom.RoomCenter;
                    Cell targetCell = comparedRoom.RoomCenter;

                    // Initialize the open and closed lists
                    PriorityQueue<Node> openList = new PriorityQueue<Node>();
                    HashSet<Cell> closedList = new HashSet<Cell>();

                    // Add the starting node to the open list
                    openList.Enqueue(new Node(startCell, null, 0, CalculateManhattanDistance(startCell, targetCell)));

                    while (openList.Count > 0)
                    {
                        // Get the node with the lowest F value from the open list
                        Node currentNode = openList.Dequeue();
                        Cell currentCell = currentNode.Cell;

                        // Check if the goal cell is reached
                        if (currentCell == targetCell)
                        {
                            // Reconstruct and return the path
                            List<Cell> path = new List<Cell>();
                            Node node = currentNode;
                            while (node != null)
                            {
                                path.Add(node.Cell);
                                node = node.Parent;
                            }
                            path.Reverse();

                            if (path.Count > longestPath.Count)
                            {
                                longestPath = path;
                                map.SpawnRoom = currentRoom;
                                map.EndRoom = comparedRoom;
                            }
                        }

                        // Mark the current cell as visited
                        closedList.Add(currentCell);

                        // Generate neighboring cells
                        int[] dx = { 0, 1, 0, -1 };
                        int[] dy = { 1, 0, -1, 0 };

                        for (int j = 0; j < 4; j++)
                        {
                            int newX = currentCell.X + dx[j];
                            int newY = currentCell.Y + dy[j];

                            Cell neighborCell = map.mapGrid[newX, newY];

                            // Skip occupied or inaccessible cells
                            if (neighborCell.Val != '░'
                                && neighborCell.Val != '#'
                                && neighborCell.Val != '·'
                                || closedList.Contains(neighborCell))
                                continue;

                            int gCost = currentNode.G + 1; // Assuming each step costs 1

                            if (!openList.Any(n => n.Cell == neighborCell) || gCost < currentNode.G) // change to evaluate the neighbour node
                            {
                                Node newNode = new Node(neighborCell, currentNode, gCost, CalculateManhattanDistance(neighborCell, targetCell));

                                openList.Enqueue(newNode);
                            }
                        }
                    }
                }
            }
        }
        static bool CheckIfIn2DArray(Cell[,] array, Cell value)
        {
            for (int row = 0; row < array.GetLength(0); row++)
            {
                for (int col = 0; col < array.GetLength(1); col++)
                {
                    if (array[row, col] == value)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        static int CalculateManhattanDistance(Cell a, Cell b)
        {
            return Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y);
        }
        private class Node : IComparable<Node>
        {
            public Cell Cell { get; }
            public Node Parent { get; }

            /// <summary>
            /// Total cost of moving from the starting position to this current node.
            /// </summary>
            public int G { get; }

            /// <summary>
            /// The distance remaining until the target position, determined by some heuristic 
            /// </summary>
            public int H { get; }

            /// <summary>
            /// The sum of the G and H values which determines how optimal this node is in relation to the optimal path
            /// </summary>
            public int F => G + H;
            public Node(Cell cell, Node parent, int g, int h)
            {
                Cell = cell;
                Parent = parent;
                G = g;
                H = h;
            }
            public int CompareTo(Node other)
            {
                return F.CompareTo(other.F);
            }
        }

        private class PriorityQueue<T> : IEnumerable<T> where T : IComparable<T>
        {
            private List<T> items = new List<T>();
            public int Count => items.Count;
            public void Enqueue(T item)
            {
                items.Add(item);
                int i = items.Count - 1;

                while (i > 0)
                {
                    int parentIndex = (i - 1) / 2;
                    if (items[i].CompareTo(items[parentIndex]) >= 0)
                        break;

                    T temp = items[i];
                    items[i] = items[parentIndex];
                    items[parentIndex] = temp;

                    i = parentIndex;
                }
            }
            public T Dequeue()
            {
                if (Count == 0)
                    throw new InvalidOperationException("PriorityQueue is empty.");

                T item = items[0];
                items[0] = items[Count - 1];
                items.RemoveAt(Count - 1);

                int i = 0;
                while (true)
                {
                    int leftChildIndex = i * 2 + 1;
                    int rightChildIndex = i * 2 + 2;

                    if (leftChildIndex >= Count)
                        break;

                    int smallerChildIndex = rightChildIndex < Count && items[rightChildIndex].CompareTo(items[leftChildIndex]) < 0
                        ? rightChildIndex
                        : leftChildIndex;

                    if (items[i].CompareTo(items[smallerChildIndex]) <= 0)
                        break;

                    T temp = items[i];
                    items[i] = items[smallerChildIndex];
                    items[smallerChildIndex] = temp;

                    i = smallerChildIndex;
                }
                return item;
            }
            public bool Contains(T item)
            {
                return items.Contains(item);
            }
            public IEnumerator<T> GetEnumerator()
            {
                return ((IEnumerable<T>)items).GetEnumerator();
            }
            IEnumerator IEnumerable.GetEnumerator()
            {
                return ((IEnumerable)items).GetEnumerator();
            }
        }
    }
}
