using System;
using System.Collections.Generic;
using System.Globalization;

using System.Xml;
using System.Xml.Linq;
using UnityEngine;
using MagicLeap.Core;

namespace SP
{
    /// <summary>
    /// Main script for initializing a serializaiton to .XML
    /// </summary>
    [ExecuteInEditMode]
    public class SceneExport : MonoBehaviour
    {
        /// <summary>
        /// XML Document that is exported
        /// </summary>
        public static XDocument doc;
        /// <summary>
        /// Initiate scene export via editor for debugging
        /// </summary>
        public bool testSceneExport = false;
        /// <summary>
        /// TCP Network Manager reference for transmission
        /// </summary>
        public NetworkManagerTCP TCP;

        private void Update() {
            // Changes this to true to be enabled
            if (testSceneExport)  {
                testSceneExport = false;
                ExportScene();
            }

        }

        /// <summary>
        /// Initiates a scene export
        /// </summary>
        public void ExportScene()
        {
            // Create arrays of all relevant objects
            var meshes = FindObjectsOfType<SPMesh>();
            var markers = FindObjectsOfType<MLArucoTrackerBehavior>();

            // Setup XML doc
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.NewLineOnAttributes = true;

            // Declare header
            doc = new XDocument(
                new XDeclaration("1.0", "utf-8", "yes"),
                new XComment(" Mapped Environment for Indoor Navigation "),
                new XComment(" Scene exported at time: " + DateTime.Now.ToString("HH:mm:ss; dd.MM.yyyy "))
            );
            print("Writing the root..");
            WriteRootNode(doc);

            // Open a dialog window for saving the output path
            XElement sceneNode = doc.Document.Root; // Get the root node

            // Write elements according to their type
            print("Writing meshes..");
            WriteMeshes(sceneNode, meshes);

            //Function to export markers (WriteMeshes as an example)
            print("Writing Markers..");
            WriteMarkers(sceneNode, markers);

            // Transmit the file over TCP
            print("Exporting the scene...");
            TCP.sendStringMessage(doc.ToString());
            
        }

        private void WriteRootNode(XDocument doc) {
            doc.Add(new XElement("Root"));
        }

        /// <summary>
        /// Write meshes to the given XML Root node
        /// </summary>
        /// <param name="XmlRoot"></param>
        /// <param name="allmeshes"></param>
        private void WriteMeshes(XElement XmlRoot, SPMesh[] allmeshes) {
            if (allmeshes.Length < 1) {
                return;
            }
            if (XmlRoot == null) {
                print("XMLRoot is null");
            }
            XmlRoot.Document.Root.Add(new XComment("Meshes"));

            foreach (var mesh in allmeshes) {
                var MeshNode = GetMeshNode(mesh);
                XmlRoot.Add(MeshNode);
            }
        }

        /// <summary>
        /// Write all markers and their positions
        /// </summary>
        /// <param name="XmlRoot"></param>
        /// <param name="allmarkers"></param>
        private void WriteMarkers(XElement XmlRoot, MLArucoTrackerBehavior[] allmarkers) {
            if (allmarkers.Length < 1) {
                return;
            }
            if (XmlRoot == null) {
                print("XMLRoot is null");
            }
            XmlRoot.Document.Root.Add(new XComment("Markers"));

            // Convert each markers pos/rot relative to global origin, add it to XML root
            foreach (var marker in allmarkers) {
                var MarkerPos = TransformConversions.posRelativeTo(GlobalOrigin.getTransform(), marker.gameObject.transform);
                var MarkerRot = TransformConversions.rotRelativeTo(GlobalOrigin.getRot(), marker.gameObject.transform.rotation).eulerAngles;

                var MeshNode = new XElement("Marker",
                          new XAttribute("id", marker.MarkerId),
                          new XAttribute("pos", MarkerPos),
                          new XAttribute("rot", MarkerRot));

                XmlRoot.Add(MeshNode);
            }

        }


        /// <summary>
        /// Get a Mesh node
        /// </summary>
        /// <param name="mesh">Mesh for which to retrieve the formatted XML node</param>
        /// <param name="transform">Optional transform</param>
        /// <returns></returns>
        private XElement GetMeshNode(SPMesh mesh) {
            // Get mesh vertices and faces
            mesh.resetVerticesNoDupesArray();
            List<Vector3> GeometryVertices = mesh.GetVertices();
            List<int> trianglePos = mesh.getMeshFacesNoDupes();

            // Calculate mesh position and rotation relative to global origin
            var meshPos = TransformConversions.posRelativeTo(GlobalOrigin.getTransform(), gameObject.transform);
            var meshRot = TransformConversions.rotRelativeTo(GlobalOrigin.getRot(), gameObject.transform.rotation).eulerAngles;

            // Writes the root mesh node with a pivot point
            var MeshNode = new XElement("Mesh",
                                new XAttribute("id", "mesh:" + mesh.gameObject.name),
                                new XAttribute("pos", meshPos),
                                new XAttribute("rot", meshRot));

            // Writes vertices
            for (int i = 0; i < GeometryVertices.Count; i++)
            {
                var vertex = GeometryVertices[i];
                MeshNode.Add(new XElement("Vertex",
                                new XAttribute("index", i),
                                new XAttribute("position", String.Format(CultureInfo.InvariantCulture,
                                "{0:F} {1:F} {2:F}",
                                vertex.x.ToString(),
                                vertex.y.ToString(),
                                vertex.z.ToString()))
                                ));
            }


            // Writes faces
            for (int c = 0, i = 0; c < trianglePos.Count; c += 3, i++)
            {
                MeshNode.Add(new XElement("Face",
                new XAttribute("index", i),
                new XAttribute("vertices", String.Format(CultureInfo.InvariantCulture,
                            "{0} {1} {2}",
                            trianglePos[c],
                            trianglePos[c + 1],
                            trianglePos[c + 2]))
                    ));
            }
            return MeshNode;
        }
    } // end of class
} // end of namespace





