using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class Utils
{
    public static bool ArrayEquals(int[] a, int[] b)
    {
        if (a == null || b == null) return false;
        if (a.Length != b.Length) return false;
        return !a.Where((t, i) => t != b[i]).Any();
    }
    /// <summary>
    /// Checks if List of visited locations have been already visited.
    /// </summary>
    /// <param name="current_location">Int array with [y,x] coordinates.</param>
    /// <param name="visitedList"></param>
    /// <returns>True/False if location has been already visited.</returns>
    public static bool HaveVisited(int[] current_location, List<int[]> visitedList)
    {
        return visitedList.Any(visitedArray => ArrayEquals(current_location, visitedArray));
    }
    /// <summary>
    /// Takes location and 2d array to get neigbours in four main directions, top, right, bottom, left.
    /// </summary>
    /// <param name="map">2d int array, in which the sides are checked.</param>
    /// <param name="x">Location of which sides are checked on X-axis.</param>
    /// <param name="y">Location of which sides are checked on X-axis.</param>
    /// <param name="locationsIgnore">List of int[] (y,x) that are ignored, return -1</param>
    /// <returns>int[] of 4 directions, top, right, bottom, left, with int identifiers as value. Returns -1 if neigbour is outside of 2d array or location is ignored.</returns>
    public static int[] GetSides(int[,] map, int x, int y, List<int[]>? locationsIgnore = null)
    {
        if (map == null) throw new Exception("Given map is null");

        // We create an array of 4 ints, each int represents a side of the wall {top, right, bottom, left}
        int[] wallSidesRooms = new int[4] { 0, 0, 0, 0 };
        // First, lets check if we can even check for walls
        if (locationsIgnore != null)
        {
            if (HaveVisited(new int[2] { y, x - 1 }, locationsIgnore))
            {
                wallSidesRooms[3] = -1;
            }
            if (HaveVisited(new int[2] { y, x + 1 }, locationsIgnore))
            {
                wallSidesRooms[1] = -1;
            }
            if (HaveVisited(new int[2] { y - 1, x }, locationsIgnore))
            {
                wallSidesRooms[0] = -1;
            }
            if (HaveVisited(new int[2] { y - 1, x }, locationsIgnore))
            {
                wallSidesRooms[2] = -1; ;
            }
        }
        if (x - 1 < 0)
        {
            wallSidesRooms[3] = -1;
        }
        if (x + 1 > map.GetLength(0) - 1)
        {
            wallSidesRooms[1] = -1;
        }
        if (y - 1 < 0)
        {
            wallSidesRooms[0] = -1;
        }
        if (y + 1 > map.GetLength(1) - 1)
        {
            wallSidesRooms[2] = -1;
        }
        int top = y - 1;
        int right = x + 1;
        int bottom = y + 1;
        int left = x - 1;
        // Now we save the identifiers of the rooms
        if (wallSidesRooms[0] != -1) { wallSidesRooms[0] = map[top, x]; }
        if (wallSidesRooms[1] != -1) { wallSidesRooms[1] = map[y, right]; }
        if (wallSidesRooms[2] != -1) { wallSidesRooms[2] = map[bottom, x]; }
        if (wallSidesRooms[3] != -1) { wallSidesRooms[3] = map[y, left]; }

        return wallSidesRooms;
    }
    /// <summary>
    /// Marks location on given map, if identifier of the location is 0 or if overwrite is true.
    /// </summary>
    /// <param name="map">2d int array where changes are made.</param>
    /// <param name="x">Location on x-axis where change is to be made.</param>
    /// <param name="y">Location on y-axis where change is to be made.</param>
    /// <param name="identifier">Int identifier that is written in specified location.</param>
    /// <param name="overwriteSpecific">Int array of identifiers that can be overwriten.</param>
    /// <param name="overwrite">Boolean that decides if everything can be overwriten or not.</param>
    /// <returns>Return updated map (2d array)</returns>
    public static int[,] MarkMap(int[,] map, int x, int y, int identifier, int[]? overwriteSpecific = null, bool overwrite = false)
    {
        if (map == null) throw new Exception("Layout is null");
        if (overwriteSpecific != null && overwriteSpecific.Contains(map[y, x]))
        {
            map[y, x] = identifier;
        }
        else if (overwrite)
        {
            map[y, x] = identifier;
        }
        else if (map[y, x] == 0)
        {
            map[y, x] = identifier;
        }
        return map;
    }

}