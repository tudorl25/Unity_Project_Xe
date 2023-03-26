using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Astar
{
    private Map map = new Map();

    private ShortestPath path = new ShortestPath();
    
    private List<Node> openList = new List<Node>();

    void getMapNodeCosts()
    {
        for (int i = 0; i < map.maxRow; i++)
        {
            for (int j = 0; j < map.maxCol; j++)
            {
                for (int z = 0; z < map.maxHeight; z++)
                {
                    map.map[i][j][z] = new Node(i,j,z);
                    
                    getCosts(map.map[i][j][z]);
                }
            }
        }
    }

    private Node startNode, goalNode, currentNode;

    public void getCosts(Node node)
    {
        int xVal = Mathf.Abs(node.x - startNode.x);
        int yVal = Mathf.Abs(node.y - startNode.y);
        int zVal = Mathf.Abs(node.z - startNode.z);

        int gCost = xVal + yVal + zVal;
        
         xVal = Mathf.Abs(node.x - goalNode.x);
         yVal = Mathf.Abs(node.y - goalNode.y);
         zVal = Mathf.Abs(node.z - goalNode.z);

         int hCost = xVal + yVal + zVal;

         int fCost = gCost + hCost;
         
         node.setCosts(gCost,hCost,fCost);
    }
    

    public void setStart(int x, int y, int z)
    {
        startNode = new Node(x, y, z);
    }

    public void setGoal(int x, int y, int z)
    {
        goalNode = new Node(x, y, z);
    }

    public bool goalReached = false;

    public void initNode(Node node)
    {
        currentNode = startNode;
        openList.Add(currentNode);
    }

    public void prepareMap(int startX, int startY, int startHeight, int goalX, int goalY, int goalHeight)
    {
        
        setStart(startX,startY,startHeight);
        
        setGoal(goalX,goalY,goalHeight);
        
        openList.Clear();
        
        path.shortestPath.Clear();
        
        getMapNodeCosts();

        goalReached = false;

    }

    public void search()
    {
        if (!goalReached)
        {
            int x = currentNode.x;
            int y = currentNode.y;
            int z = currentNode.z;

            openList.Remove(currentNode);
            currentNode.setChecked();
            
            
            /*
             *Treaba migaloasa, de dat la altcineva 
             */
            openNode(map.map[x-1][y][z]);
            
            openNode(map.map[x+1][y][z]); 
            
            openNode(map.map[x][y-1][z]);
            
            openNode(map.map[x][y+1][z]);

            int bestNodeIndex = 0;
            int bestNodeCost = 999;

            for (int i = 0; i < openList.Count; i++)
            {
                if (openList[i].fCost < bestNodeCost)
                {
                    bestNodeCost = openList[i].fCost;
                    bestNodeIndex = i;
                }else if (openList[i].fCost == bestNodeCost)
                {
                    if (openList[i].gCost < openList[bestNodeIndex].gCost)
                    {
                        bestNodeIndex = i;
                    }
                }
            }

            currentNode = openList[bestNodeIndex];

            if (currentNode == goalNode)
            {
                goalReached = true;
            }


        }
    }

    public void openNode(Node node)
    {
        if (!node.solid && !node.traversed && !node.open)
        {
            node.setOpen();
            openList.Add(node);
            node.parent = currentNode;
        }
    }

    public void getPath()
    {
        Node current;
        
        current = goalNode.parent;

        while (current != startNode)
        {
            path.shortestPath.Add(current.parent);

            current = current.parent;
        }
    }
    
}
