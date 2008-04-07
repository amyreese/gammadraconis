using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using GammaDraconis.Types;

class OctreeLeaf
{
    private const int maxobj = 50;
    private List<GameObject> containedObjects;
    public List<OctreeLeaf> childLeaves;
    private BoundingBox containerBox;

    public OctreeLeaf(BoundingBox bound)
    {
        containedObjects = new List<GameObject>();
        childLeaves = new List<OctreeLeaf>(8);
        containerBox = bound;
    }
    
    public void setContainedObjects(List<GameObject> value)
    {
        containedObjects = value;
        //Split();
    }

    public List<GameObject> getContainedObjects()
    {
        return containedObjects;
    }

    public List<OctreeLeaf> getChildLeaves()
    {
        return childLeaves;
    }

    public BoundingBox getContainerBox()
    {
        return containerBox;
    }
    
    public void setContainerBox(BoundingBox value)
    {
        containerBox = value;
    }

    protected void Split()
    {
        Vector3 half = containerBox.Max - containerBox.Min;
        Vector3 halfx = Vector3.UnitX * half;
        Vector3 halfy = Vector3.UnitY * half;
        Vector3 halfz = Vector3.UnitZ * half;
        BoundingBox[] boxes = {
            new BoundingBox(containerBox.Min, containerBox.Min + half), 
            new BoundingBox(containerBox.Min + halfx, containerBox.Max - half + halfx),
            new BoundingBox(containerBox.Min + halfz, containerBox.Min + half + halfz),
            new BoundingBox(containerBox.Min + halfx + halfz, containerBox.Max - halfy),
            new BoundingBox(containerBox.Min + halfy, containerBox.Max - halfx - halfz),
            new BoundingBox(containerBox.Min + halfy + halfx, containerBox.Max - halfz),
            new BoundingBox(containerBox.Min + halfy + halfz, containerBox.Max - halfx),
            new BoundingBox(containerBox.Min + half, containerBox.Max)
        };
        
        foreach( BoundingBox tempBox in boxes)
        {
            OctreeLeaf tempLeaf = new OctreeLeaf(tempBox);
            foreach(GameObject obj in containedObjects){
                if (tempBox.Contains(new BoundingSphere(obj.position.pos(), obj.size)) != ContainmentType.Disjoint){
                    tempLeaf.containedObjects.Add(obj);
                }
            }
            /*if (tempLeaf.containedObjects.Count > maxobj){
                tempLeaf.Split();
            }*/
            childLeaves.Add(tempLeaf);
        }
            
    }
   
}