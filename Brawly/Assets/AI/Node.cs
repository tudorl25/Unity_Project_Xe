using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using UnityEngine;

public class Node
{
    public int x;
    public int y;
    public int z;

   public bool traversed = false;
     
   public bool open = false;

   public Node(int x, int y, int z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }

   public int gCost;
   public int hCost;
   public int fCost;

   public bool solid = false;

   public Node parent;

   public void setChecked()
    {
        traversed = true;
    }

   public void setOpen()
    {
        open = true;
    }

   public void setSolid()
   {
       solid = true;
   }

   public void setCosts(int gCost, int hCost, int fCost)
   {
       this.gCost = gCost;
       this.hCost = hCost;
       this.fCost = fCost;
   }
    
    
}
