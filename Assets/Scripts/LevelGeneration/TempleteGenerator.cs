using System;
using System.Collections.Generic;

public class TempleteGenerator
{
    private int levelWidthCells = 9;
    private int levelHeightCells = 9;

    private int numberOfEnemyRooms = 3;
    private int enemyRoomMaxCellDistFromEdge = 3;
    private int minCellDistBetweenEnemyRooms = 1;

    private int numberOfInvisibleObstacles = 8;
    private int minCellDistBetweenInvisibleObstacles = 1;

    /*
     * 0-nothing
     * 1-start room
     * 2-enemy rooms
     * 3-obstacles, to make the corridors more undulating
     * 4-corridor
     * 5-boss room
     */

    private int[,] world;

    private Coords startRoomCoords;
    private Coords[] enemyRoomsCoords;

    public TempleteGenerator(int levelWidthCells, int levelHeightCells, int numberOfEnemyRooms, int enemyRoomMaxCellDistFromEdge, int minCellDistBetweenEnemyRooms, int numberOfInvisibleObstacles, int minCellDistBetweenInvisibleObstacles)
    {
        this.levelWidthCells = levelWidthCells;
        this.levelHeightCells = levelHeightCells;
        this.numberOfEnemyRooms = numberOfEnemyRooms;
        this.enemyRoomMaxCellDistFromEdge = enemyRoomMaxCellDistFromEdge;
        this.minCellDistBetweenEnemyRooms = minCellDistBetweenEnemyRooms;
        this.numberOfInvisibleObstacles = numberOfInvisibleObstacles;
        this.minCellDistBetweenInvisibleObstacles = minCellDistBetweenInvisibleObstacles;
        world = new int[levelWidthCells, levelHeightCells];
        enemyRoomsCoords = new Coords[numberOfEnemyRooms];
    }

    public int[,] GetTemplete()
    {
        // Temlete is too small
        if(numberOfInvisibleObstacles > (levelHeightCells * levelWidthCells / 10))
        {
            return null;
        }
        if (numberOfEnemyRooms > (levelHeightCells * levelWidthCells / 15))
        {
            return null;
        }
        try
        {
            GenerateStartRoom(world);
            GenerateEnemyRooms(world, numberOfEnemyRooms, enemyRoomMaxCellDistFromEdge, minCellDistBetweenEnemyRooms);
            GenerateObstacles(world, numberOfInvisibleObstacles, minCellDistBetweenInvisibleObstacles);
            GenerateCorridors(world);
            GenerateBossRoom(world);
            RemoveObstacles(world);
        } catch
        {
            return null;
        }
        return world;
    }

    // Generate start room at the center
    private void GenerateStartRoom(int[,] world)
    {
        world[world.GetLength(0) / 2, world.GetLength(1) / 2] = 1;
        startRoomCoords = new Coords(world.GetLength(0) / 2, world.GetLength(1) / 2);
    }

    // Generate enemy rooms
    private void GenerateEnemyRooms(int[,] world, int numberOfEnemyRooms, int maxDistFromEdges, int minDistBetweenRooms)
    {

        for (int i = 0; i < numberOfEnemyRooms; i++)
        {
            int roomX, roomY;
            do
            {
                roomX = RandomNumberIn2Ranges(0, maxDistFromEdges, world.GetLength(0) - maxDistFromEdges, world.GetLength(0));
                roomY = RandomNumberIn2Ranges(0, maxDistFromEdges, world.GetLength(1) - maxDistFromEdges, world.GetLength(1));
            } while (world[roomX, roomY] > 0 || PointHasRoomInRadius(world, roomX, roomY, 2, minDistBetweenRooms));
            world[roomX, roomY] = 2;
            enemyRoomsCoords[i] = new Coords(roomX, roomY);
        }
    }

    // Generate obstacles to make the corridors more undulating
    private void GenerateObstacles(int[,] world, int numberOfObstacles, int minDistBetweebObstacles)
    {

        for (int i = 0; i < numberOfObstacles; i++)
        {
            int roomX, roomY;
            Random rnd = new Random();
            do
            {
                roomX = rnd.Next(world.GetLength(0));
                roomY = rnd.Next(world.GetLength(1));
            } while (world[roomX, roomY] > 0 || PointHasRoomInRadius(world, roomX, roomY, 3, minDistBetweebObstacles) || PointHasRoomInRadius(world, roomX, roomY, 1, 1) || PointHasRoomInRadius(world, roomX, roomY, 2, 1));
            world[roomX, roomY] = 3;
        }
    }

    private void GenerateCorridors(int[,] world)
    {
        bool[,] obstacles = new bool[world.GetLength(0), world.GetLength(1)];
        for (int i = 0; i < world.GetLength(0); i++)
        {
            for (int j = 0; j < world.GetLength(1); j++)
            {
                if (world[i, j] > 0 && world[i, j] != 4) obstacles[i, j] = true;
            }
        }
        for (int i = 0; i < enemyRoomsCoords.GetLength(0); i++)
        {
            obstacles[enemyRoomsCoords[i].X, enemyRoomsCoords[i].Y] = false;
        }
        // Create corridors with pathfinding
        for (int i = 0; i < enemyRoomsCoords.GetLength(0); i++)
        {
            Pathfinder pf = new Pathfinder(0, 0, world.GetLength(0), world.GetLength(1), obstacles);
            List<Coords> coords = pf.GetPath(startRoomCoords.X, startRoomCoords.Y, enemyRoomsCoords[i].X, enemyRoomsCoords[i].Y);
            for (int w = 0; w < coords.Count; w++)
            {
                if (world[coords[w].X, coords[w].Y] != 2)
                    world[coords[w].X, coords[w].Y] = 4;
            }
        }
    }

    private void GenerateBossRoom(int[,] world)
    {
        bool[,] obstacles = new bool[world.GetLength(0), world.GetLength(1)];
        for (int i = 0; i < world.GetLength(0); i++)
        {
            for (int j = 0; j < world.GetLength(1); j++)
            {
                if (world[i, j] > 0 && world[i, j] != 4) obstacles[i, j] = true;
            }
        }
        for (int i = 0; i < enemyRoomsCoords.GetLength(0); i++)
        {
            obstacles[enemyRoomsCoords[i].X, enemyRoomsCoords[i].Y] = false;
        }
        int roomX, roomY;
        Random rnd = new Random();
        do
        {
            roomX = rnd.Next(world.GetLength(0));
            roomY = rnd.Next(world.GetLength(1));
        } while (world[roomX, roomY] > 0 || PointHasRoomInRadius(world, roomX, roomY, 1, 1) || PointHasRoomInRadius(world, roomX, roomY, 1, 1) || PointHasRoomInRadius(world, roomX, roomY, 2, 1));
        world[roomX, roomY] = 5;

        // Corridor from first eneny room to boss room
        Pathfinder pf = new Pathfinder(0, 0, world.GetLength(0), world.GetLength(1), obstacles);
        List<Coords> coords = pf.GetPath(roomX, roomY, enemyRoomsCoords[0].X, enemyRoomsCoords[0].Y);
        for (int w = 0; w < coords.Count; w++)
        {
            if (world[coords[w].X, coords[w].Y] != 2)
                world[coords[w].X, coords[w].Y] = 4;
        }

    }

    private void RemoveObstacles(int[,] world)
    {
        for (int i = 0; i < world.GetLength(0); i++)
        {
            for (int j = 0; j < world.GetLength(1); j++)
            {
                if (world[i, j] == 3) world[i, j] = 0;
            }
        }
    }

    private bool PointHasRoomInRadius(int[,] world, int pointX, int pointY, int roomType, int radius)
    {

        int searhMinX = Math.Max(0, pointX - radius);
        int searhMinY = Math.Max(0, pointY - radius);
        int searhMaxX = Math.Min(world.GetLength(1) - 1, pointX + radius);
        int searhMaxY = Math.Min(world.GetLength(1) - 1, pointY + radius);

        for (int i = searhMinX; i <= searhMaxX; i++)
        {
            for (int j = searhMinY; j <= searhMaxY; j++)
            {
                if (world[i, j] == roomType) return true;
            }
        }

        return false;
    }

    private int RandomNumberIn2Ranges(int from1, int to1, int from2, int to2)
    {
        Random rnd = new Random();
        bool isInFirstRange = rnd.NextDouble() >= 0.5;
        if (isInFirstRange) return rnd.Next(from1, to1);
        return rnd.Next(from2, to2);
    }

}