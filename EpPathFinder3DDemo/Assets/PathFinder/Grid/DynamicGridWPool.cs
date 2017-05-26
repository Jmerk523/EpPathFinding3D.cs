﻿/*! 
@file DynamicGridWPool.cs
@author Woong Gyu La a.k.a Chris. <juhgiyo@gmail.com>
		<http://github.com/juhgiyo/eppathfinding3d.cs>
@date April 20, 2017
@brief DynamicGrid with Pool Interface
@version 2.0

@section LICENSE

The MIT License (MIT)

Copyright (c) 2017 Woong Gyu La <juhgiyo@gmail.com>

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.

@section DESCRIPTION

An Interface for the DynamicGrid with Pool Class.

*/
using System;
using System.Collections.Generic;
using System.Collections;


namespace EpPathFinding3D.cs
{
    public class DynamicGridWPool : BaseGrid
    {
         private bool m_notSet;
         private NodePool m_nodePool;

        public override int width
        {
            get
            {
                if (m_notSet)
                    setBoundingBox();
                return m_gridRect.maxX - m_gridRect.minX;
            }
            protected set
            {

            }
        }

        public override int length
        {
            get
            {
                if (m_notSet)
                    setBoundingBox();
                return m_gridRect.maxY - m_gridRect.minY;
            }
            protected set
            {

            }
        }
        public override int height
        {
            get
            {
                if (m_notSet)
                    setBoundingBox();
                return m_gridRect.maxZ - m_gridRect.minZ;
            }
            protected set
            {

            }
        }

        public DynamicGridWPool(NodePool iNodePool)
            : base()
        {
            m_gridRect = new GridCube();
            m_gridRect.minX = 0;
            m_gridRect.minY = 0;
            m_gridRect.maxX = 0;
            m_gridRect.maxY = 0;
            m_gridRect.minZ = 0;
            m_gridRect.maxZ = 0;
            m_notSet = true;
            m_nodePool = iNodePool;
        }

        public DynamicGridWPool(DynamicGridWPool b)
            : base(b)
        {
            m_notSet = b.m_notSet;
            m_nodePool = b.m_nodePool;
        }

        public override Node GetNodeAt(int iX, int iY, int iZ)
        {
            GridPos pos = new GridPos(iX, iY, iZ);
            return GetNodeAt(pos);
        }

        public override bool IsWalkableAt(int iX, int iY, int iZ)
        {
            GridPos pos = new GridPos(iX, iY, iZ);
            return IsWalkableAt(pos);
        }

        private void setBoundingBox()
        {
              foreach (KeyValuePair<GridPos, Node> pair in m_nodePool.Nodes)
            {
                if (pair.Key.x < m_gridRect.minX || m_notSet)
                    m_gridRect.minX = pair.Key.x;
                if (pair.Key.x > m_gridRect.maxX || m_notSet)
                    m_gridRect.maxX = pair.Key.x;
                if (pair.Key.y < m_gridRect.minY || m_notSet)
                    m_gridRect.minY = pair.Key.y;
                if (pair.Key.y > m_gridRect.maxY || m_notSet)
                    m_gridRect.maxY = pair.Key.y;
                if (pair.Key.z < m_gridRect.minZ || m_notSet)
                    m_gridRect.minZ = pair.Key.z;
                if (pair.Key.z > m_gridRect.maxZ || m_notSet)
                    m_gridRect.maxZ = pair.Key.z;
                m_notSet = false;
            }
            m_notSet = false;
        }

        public override bool SetWalkableAt(int iX, int iY, int iZ, bool iWalkable)
        {
            GridPos pos = new GridPos(iX, iY, iZ);
            m_nodePool.SetNode(pos, iWalkable);
            if (iWalkable)
            {
                if (iX < m_gridRect.minX || m_notSet)
                    m_gridRect.minX = iX;
                if (iX > m_gridRect.maxX || m_notSet)
                    m_gridRect.maxX = iX;
                if (iY < m_gridRect.minY || m_notSet)
                    m_gridRect.minY = iY;
                if (iY > m_gridRect.maxY || m_notSet)
                    m_gridRect.maxY = iY;
                if (iZ < m_gridRect.minZ || m_notSet)
                    m_gridRect.minZ = iZ;
                if (iZ > m_gridRect.maxZ || m_notSet)
                    m_gridRect.maxZ = iZ;
                m_notSet = false;
            }
            else
            {
                if (iX == m_gridRect.minX || iX == m_gridRect.maxX || iY == m_gridRect.minY || iY == m_gridRect.maxY || iZ == m_gridRect.minZ || iZ == m_gridRect.maxZ)
                    m_notSet = true;
                
            }
            return true;
        }

        public override Node GetNodeAt(GridPos iPos)
        {
            return m_nodePool.GetNode(iPos);
        }

        public override bool IsWalkableAt(GridPos iPos)
        {
            return  m_nodePool.Nodes.ContainsKey(iPos);
        }

        public override bool SetWalkableAt(GridPos iPos, bool iWalkable)
        {
            return SetWalkableAt(iPos.x, iPos.y, iPos.z, iWalkable);
        }


        public override void Reset()
        {
            foreach (KeyValuePair<GridPos, Node> keyValue in m_nodePool.Nodes)
            {
                keyValue.Value.Reset();
            }
        }

        public override BaseGrid Clone()
        {
            DynamicGridWPool tNewGrid = new DynamicGridWPool(m_nodePool);
            return tNewGrid;
        }
    }

}