/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2018, the respective contributors. All rights reserved.
 *
 * Each contributor holds copyright over their respective contributions.
 * The project versioning (Git) records all such contribution source information.
 *                                           
 *                                                                              
 * The BHoM is free software: you can redistribute it and/or modify         
 * it under the terms of the GNU Lesser General Public License as published by  
 * the Free Software Foundation, either version 3.0 of the License, or          
 * (at your option) any later version.                                          
 *                                                                              
 * The BHoM is distributed in the hope that it will be useful,              
 * but WITHOUT ANY WARRANTY; without even the implied warranty of               
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the                 
 * GNU Lesser General Public License for more details.                          
 *                                                                            
 * You should have received a copy of the GNU Lesser General Public License     
 * along with this code. If not, see <https://www.gnu.org/licenses/lgpl-3.0.html>.      
 */

using System;
using System.Collections.Generic;
using System.Linq;
using RHG = Rhino.Geometry;
using BHG = BH.oM.Geometry;

namespace BH.Engine.Rhinoceros
{
    public static partial class Convert
    {
        /***************************************************/
        /**** Public Methods  - Interfaces              ****/
        /***************************************************/

        public static BHG.IGeometry IToBHoM(this RHG.GeometryBase geometry)
        {
            return (geometry == null) ? null : Convert.ToBHoM(geometry as dynamic);
        }

        /***************************************************/

        public static BHG.IGeometry IToBHoM<T>(this Rhino.IEpsilonComparable<T> geometry)
        {
            return (geometry == null) ? null : Convert.ToBHoM(geometry as dynamic);
        }


        /***************************************************/
        /**** Public Methods  - Vectors                 ****/
        /***************************************************/

        public static BHG.Point ToBHoM(this RHG.Point3d rhinoPoint)
        {
            return new BHG.Point { X = rhinoPoint.X, Y = rhinoPoint.Y, Z = rhinoPoint.Z };
        }

        /***************************************************/

        public static BHG.Point ToBHoM(this RHG.Point3f rhinoPoint)
        {
            return new BHG.Point { X = rhinoPoint.X, Y = rhinoPoint.Y, Z = rhinoPoint.Z };
        }

        /***************************************************/

        public static BHG.Point ToBHoM(this RHG.Point rhinoPoint)
        {
            if (rhinoPoint == null) return null;

            return new BHG.Point { X = rhinoPoint.Location.X, Y = rhinoPoint.Location.Y, Z = rhinoPoint.Location.Z };
        }

        /***************************************************/

        public static BHG.Point ToBHoM(this RHG.ControlPoint rhinoPoint)
        {
            return new BHG.Point { X = rhinoPoint.Location.X, Y = rhinoPoint.Location.Y, Z = rhinoPoint.Location.Z };
        }

        /***************************************************/

        public static BHG.Point ToBHoM(this RHG.BrepVertex vertex)
        {
            return new BHG.Point { X = vertex.Location.X, Y = vertex.Location.Y, Z = vertex.Location.Z };
        }

        /***************************************************/

        public static BHG.Vector ToBHoM(this RHG.Vector3d vector)
        {
            return new BHG.Vector { X = vector.X, Y = vector.Y, Z = vector.Z };
        }

        /***************************************************/

        public static BHG.Vector ToBHoM(this RHG.Vector3f vector)
        {
            return new BHG.Vector { X = vector.X, Y = vector.Y, Z = vector.Z };
        }

        /***************************************************/

        public static BHG.CoordinateSystem.Cartesian ToBHoM(this RHG.Plane plane)
        {
            return Geometry.Create.CartesianCoordinateSystem(plane.Origin.ToBHoM(), plane.XAxis.ToBHoM(), plane.YAxis.ToBHoM());
        }

        /***************************************************/

        public static BHG.Quaternion ToBHoM(this RHG.Quaternion quaternion)
        {
            return new BHG.Quaternion
            {
                X = quaternion.A,
                Y = quaternion.B,
                Z = quaternion.C,
                W = quaternion.D
            };
        }

        /***************************************************/

        public static BHG.TransformMatrix ToBHoM(this RHG.Transform rhTrans)
        {
            BHG.TransformMatrix bhTrans = new BHG.TransformMatrix();
            bhTrans.Matrix[0, 0] = rhTrans[0, 0];
            bhTrans.Matrix[0, 1] = rhTrans[0, 1];
            bhTrans.Matrix[0, 2] = rhTrans[0, 2];
            bhTrans.Matrix[0, 3] = rhTrans[0, 3];

            bhTrans.Matrix[1, 0] = rhTrans[1, 0];
            bhTrans.Matrix[1, 1] = rhTrans[1, 1];
            bhTrans.Matrix[1, 2] = rhTrans[1, 2];
            bhTrans.Matrix[1, 3] = rhTrans[1, 3];

            bhTrans.Matrix[2, 0] = rhTrans[2, 0];
            bhTrans.Matrix[2, 1] = rhTrans[2, 1];
            bhTrans.Matrix[2, 2] = rhTrans[2, 2];
            bhTrans.Matrix[2, 3] = rhTrans[2, 3];

            bhTrans.Matrix[3, 0] = rhTrans[3, 0];
            bhTrans.Matrix[3, 1] = rhTrans[3, 1];
            bhTrans.Matrix[3, 2] = rhTrans[3, 2];
            bhTrans.Matrix[3, 3] = rhTrans[3, 3];

            return bhTrans;
        }


        /***************************************************/
        /**** Public Methods  - Curves                  ****/
        /***************************************************/

        public static BHG.Arc ToBHoM(this RHG.Arc arc)
        {
            BHG.CoordinateSystem.Cartesian system = arc.Plane.ToBHoM();

            return new BHG.Arc { CoordinateSystem = system, StartAngle = arc.StartAngle, EndAngle = arc.EndAngle, Radius = arc.Radius };
        }

        /***************************************************/

        public static BHG.ICurve ToBHoM(this RHG.ArcCurve arcCurve)
        {
            if (arcCurve == null) return null;

            if (arcCurve.IsCompleteCircle)
            {
                RHG.Circle circle;
                arcCurve.TryGetCircle(out circle);
                return circle.ToBHoM();
            }
            else
                return arcCurve.Arc.ToBHoM();
        }

        /***************************************************/

        public static BHG.Circle ToBHoM(this RHG.Circle circle)
        {
            return new BHG.Circle { Centre = circle.Center.ToBHoM(), Normal = circle.Normal.ToBHoM(), Radius = circle.Radius };
        }

        /***************************************************/

        public static BHG.Ellipse ToBHoM(this RHG.Ellipse ellipse)
        {
            return new BHG.Ellipse
            {
                Centre = ellipse.Plane.Origin.ToBHoM(),
                Axis1 = ellipse.Plane.XAxis.ToBHoM(),
                Axis2 = ellipse.Plane.YAxis.ToBHoM(),
                Radius1 = ellipse.Radius1,
                Radius2 = ellipse.Radius2
            };
        }

        /***************************************************/

        public static BHG.Line ToBHoM(this RHG.Line line)
        {
            return new BHG.Line { Start = line.From.ToBHoM(), End = line.To.ToBHoM() };
        }

        /***************************************************/

        public static BHG.Line ToBHoM(this RHG.LineCurve line)
        {
            if (line == null) return null;

            return new BHG.Line { Start = line.PointAtStart.ToBHoM(), End = line.PointAtEnd.ToBHoM() };
        }

        /***************************************************/

        public static BHG.ICurve ToBHoM(this RHG.NurbsCurve rCurve)
        {
            if (rCurve == null) return null;

            if (rCurve.IsPolyline())
            {
                RHG.Polyline polyline;
                rCurve.TryGetPolyline(out polyline);
                return polyline.ToBHoM();
            }

            if (rCurve.IsClosed && rCurve.IsEllipse())
            {
                RHG.Ellipse ellipse = new RHG.Ellipse();
                rCurve.TryGetEllipse(out ellipse);
                return ellipse.ToBHoM();
            }

            IEnumerable<RHG.ControlPoint> rPoints = rCurve.Points;
            List<double> knots = rCurve.Knots.ToList();
            return new BHG.NurbsCurve
            {
                ControlPoints = rPoints.Select(x => x.ToBHoM()).ToList(),
                Weights = rPoints.Select(x => x.Weight).ToList(),
                Knots = knots
            };
        }

        /***************************************************/

        public static BHG.ICurve ToBHoM(this RHG.Curve rCurve)
        {
            if (rCurve == null) return null;

            Type curveType = rCurve.GetType();
            if (rCurve.IsLinear() && rCurve.SpanCount < 2)
            {
                return new BHG.Line { Start = rCurve.PointAtStart.ToBHoM(), End = rCurve.PointAtEnd.ToBHoM(), Infinite = false };
            }
            if (rCurve.IsCircle())
            {
                RHG.Circle circle = new RHG.Circle();
                rCurve.TryGetCircle(out circle);
                return circle.ToBHoM();
            }
            else if (rCurve.IsArc() || typeof(RHG.ArcCurve).IsAssignableFrom(curveType))
            {
                RHG.Arc arc = new RHG.Arc();
                rCurve.TryGetArc(out arc);
                return arc.ToBHoM();
            }
            else if (rCurve.IsPolyline() || typeof(RHG.PolylineCurve).IsAssignableFrom(curveType))
            {
                RHG.Polyline polyline = new RHG.Polyline();
                rCurve.TryGetPolyline(out polyline);
                return polyline.ToBHoM();
            }
            else if (rCurve.IsClosed && rCurve.IsEllipse())
            {
                RHG.Ellipse ellipse = new RHG.Ellipse();
                rCurve.TryGetEllipse(out ellipse);
                return ellipse.ToBHoM();
            }
            else if (rCurve is RHG.NurbsCurve)
            {
                return ((RHG.NurbsCurve)rCurve).ToBHoM();
            }
            else if (rCurve is RHG.PolyCurve)
            {
                return ((RHG.PolyCurve)rCurve).ToBHoM();  //The test of IsPolyline above is very important to make sure we can cast to PolyCurve here
            }
            else
            {
                return (rCurve.ToNurbsCurve()).ToBHoM();
            }
        }

        /***************************************************/

        public static BHG.ICurve ToBHoM(this RHG.PolyCurve polyCurve)
        {
            if (polyCurve == null) return null;

            polyCurve.RemoveNesting();
            if (polyCurve.IsPolyline())
            {
                RHG.Polyline polyline;
                polyCurve.TryGetPolyline(out polyline);
                return polyline.ToBHoM();
            }
            else
                return new BHG.PolyCurve { Curves = polyCurve.Explode().Select(x => x.ToBHoM()).ToList() };
        }

        /***************************************************/

        public static BHG.Polyline ToBHoM(this RHG.Polyline polyline)
        {
            if (polyline == null) return null;

            return new BHG.Polyline { ControlPoints = polyline.Select(x => x.ToBHoM()).ToList() };
        }

        /***************************************************/

        public static BHG.Polyline ToBHoM(this RHG.PolylineCurve polyline)
        {
            if (polyline == null) return null;

            if (!polyline.IsPolyline()) { return null; }
            RHG.Polyline rPolyline; polyline.TryGetPolyline(out rPolyline);
            return rPolyline.ToBHoM();
        }


        /***************************************************/
        /**** Public Methods  - Surfaces                ****/
        /***************************************************/

        public static BHG.BoundingBox ToBHoM(this RHG.BoundingBox boundingBox)
        {
            return new BHG.BoundingBox { Min = boundingBox.Min.ToBHoM(), Max = boundingBox.Max.ToBHoM() };
        }

        /***************************************************/

        public static BHG.BoundingBox ToBHoM(this RHG.Box box)
        {
            return box.BoundingBox.ToBHoM();
        }

        /***************************************************/

        public static BHG.ISurface ToBHoM(this RHG.Surface surface)
        {
            if (surface == null) return null;

            return surface.ToNurbsSurface().ToBHoM();
        }

        /***************************************************/

        public static BHG.NurbsSurface ToBHoM(this RHG.NurbsSurface surface)
        {
            if (surface == null) return null;

            return new BHG.NurbsSurface
            {
                ControlPoints = surface.Points.Select(x => x.Location.ToBHoM()).ToList(),
                Weights = surface.Points.Select(x => x.Weight).ToList(),
                UKnots = surface.KnotsU.ToList(),
                VKnots = surface.KnotsV.ToList()
            };
        }

        /***************************************************/

        public static BHG.IGeometry ToBHoM(this RHG.Brep brep)
        {
            if (brep == null) return null;

            if (brep.Surfaces.Count == 0) return null;


            if (brep.IsSolid) return brep.ToBHoM(true);
        

            if (brep.IsPlanarSurface())
            {
                BHG.ICurve externalEdge = RHG.Curve.JoinCurves(brep.DuplicateNakedEdgeCurves(true, false)).FirstOrDefault().ToBHoM();
                List<BHG.ICurve> internalEdges = RHG.Curve.JoinCurves(brep.DuplicateNakedEdgeCurves(false, true)).Select(c => c.ToBHoM()).ToList();
                return new BHG.PlanarSurface { ExternalBoundary = externalEdge, InternalBoundaries = internalEdges };
            }

            // Default case - return open Polysurface
            return new BHG.PolySurface() { Surfaces = brep.Surfaces.Select(s => s.ToBHoM()).ToList() };
        }

        /***************************************************/

        public static BHG.Extrusion ToBHoM(this RHG.Extrusion extrusion)
        {
            if (extrusion == null) return null;

            extrusion.PathLineCurve();
            throw new NotImplementedException(); // TODO Rhino_Adapter conversion from Extrusion
        }


        /***************************************************/
        /**** Public Methods  - Mesh                    ****/
        /***************************************************/

        public static BHG.Mesh ToBHoM(this RHG.Mesh rMesh)
        {
            if (rMesh == null) return null;

            List<BHG.Point> vertices = rMesh.Vertices.ToList().Select(x => x.ToBHoM()).ToList();
            List<RHG.MeshFace> rFaces = rMesh.Faces.ToList();
            List<BHG.Face> faces = new List<BHG.Face>();
            for (int i = 0; i < rFaces.Count; i++)
            {
                if (rFaces[i].IsQuad)
                {
                    faces.Add(new BHG.Face { A = rFaces[i].A, B = rFaces[i].B, C = rFaces[i].C, D = rFaces[i].D });
                }
                if (rFaces[i].IsTriangle)
                {
                    faces.Add(new BHG.Face { A = rFaces[i].A, B = rFaces[i].B, C = rFaces[i].C });
                }
            }
            return new BHG.Mesh { Vertices = vertices, Faces = faces };
        }

        /***************************************************/

        public static BHG.Face ToBHoM(this RHG.MeshFace rFace)
        {

            BHG.Face face = new BHG.Face
            {
                A = rFace.A,
                B = rFace.B,
                C = rFace.C
            };

            if (rFace.IsQuad)
                face.D = rFace.D;

            return face;
        }

        /***************************************************/
        /**** Public Methods  - Solids                  ****/
        /***************************************************/


        private static BHG.ISolid ToBHoM(this RHG.Brep brep, bool isSolid)
        {
            RHG.Surface surface = brep.Surfaces.FirstOrDefault();
            switch (brep.Surfaces.Count)
            {
                case 1:
                    RHG.Sphere sphere;
                    if (surface.TryGetSphere(out sphere))
                        return sphere.ToBHoM();
                    break;
                case 2:
                    RHG.Cone cone;
                    if (surface.TryGetCone(out cone))
                        return cone.ToBHoM();
                    break;
                case 3:
                    RHG.Cylinder cylinder;
                    if (surface.TryGetCylinder(out cylinder))
                        return cylinder.ToBHoM();
                    break;
            }

            return new BHG.BoundaryRepresentation(brep.Surfaces.Select(s => s.ToBHoM()).ToList());
        }

        /***************************************************/

        public static BHG.Sphere ToBHoM(this RHG.Sphere sphere)
        {
            return new BHG.Sphere { Centre = sphere.Center.ToBHoM(), Radius = sphere.Radius };
        }

        /***************************************************/

        public static BHG.Torus ToBHoM(this RHG.Torus torus)
        {
            return new BHG.Torus { Centre = torus.Plane.Origin.ToBHoM(), Axis = torus.Plane.ZAxis.ToBHoM(), RadiusMajor = torus.MajorRadius, RadiusMinor = torus.MinorRadius };
        }

        /***************************************************/

        public static BHG.Cone ToBHoM(this RHG.Cone cone)
        {
            return new BHG.Cone { Centre = cone.BasePoint.ToBHoM(), Axis = cone.Axis.ToBHoM(), Radius = cone.Radius, Height = cone.Height };
        }

        /***************************************************/

        public static BHG.Cylinder ToBHoM(this RHG.Cylinder cylinder)
        {
            BHG.Point centre = cylinder.Center.ToBHoM() + cylinder.Axis.ToBHoM() * cylinder.Height1;
            return new BHG.Cylinder { Centre = centre, Axis = cylinder.Axis.ToBHoM(), Height = cylinder.TotalHeight, Radius = cylinder.CircleAt(0.0).Radius };
        }


        /***************************************************/
        /**** Miscellanea                               ****/
        /***************************************************/

        public static BHG.CompositeGeometry ToBHoM(this List<RHG.GeometryBase> geometries)
        {
            return new BHG.CompositeGeometry { Elements = geometries.Select(x => x.IToBHoM()).ToList() };
        }

        /***************************************************/
    }
}
