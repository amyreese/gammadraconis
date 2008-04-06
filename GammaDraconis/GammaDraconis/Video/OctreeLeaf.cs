using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using GammaDraconis.Types;

class OctreeLeaf
{
    private const int maxobj = 8;
    private List<GameObject> containedObjects;
    private List<OctreeLeaf> childLeaves;
    private BoundingBox containerBox;

    public OctreeLeaf(BoundingBox bound)
    {
        containedObjects = new List<GameObject>();
        childLeaves = null;
        containerBox = bound;
    }

    public List<GameObject> ContainedObjects
    {
        get { return containedObjects; }
        set { containedObjects = value; }
    }

    public List<OctreeLeaf> ChildLeaves
    {
        get { return childLeaves; }
    }

    public BoundingBox ContainerBox
    {
        get { return containerBox; }
        set { containerBox = value; }
    }

    protected void Split()
    {
        Vector3 half = ContainerBox.Max - ContainerBox.Min;
        Vector3 halfx = Vector3.UnitX * half;
        Vector3 halfy = Vector3.UnitY * half;
        Vector3 halfz = Vector3.UnitZ * half;

        ChildLeaves.Add(new OctreeLeaf(new BoundingBox(ContainerBox.Min, ContainerBox.Min + half)));
        ChildLeaves.Add(new OctreeLeaf(new BoundingBox(ContainerBox.Min + halfx, ContainerBox.Max - half + halfx)));
        ChildLeaves.Add(new OctreeLeaf(new BoundingBox(ContainerBox.Min + halfz, ContainerBox.Min + half + halfz)));
        ChildLeaves.Add(new OctreeLeaf(new BoundingBox(ContainerBox.Min + halfx + halfz, ContainerBox.Max - halfy)));
        ChildLeaves.Add(new OctreeLeaf(new BoundingBox(ContainerBox.Min + halfy, ContainerBox.Max - halfx - halfz)));
        ChildLeaves.Add(new OctreeLeaf(new BoundingBox(ContainerBox.Min + halfy + halfx, ContainerBox.Max - halfz)));
        ChildLeaves.Add(new OctreeLeaf(new BoundingBox(ContainerBox.Min + halfy + halfz, ContainerBox.Max - halfx)));
        ChildLeaves.Add(new OctreeLeaf(new BoundingBox(ContainerBox.Min + half, ContainerBox.Max)));
    }
   
}