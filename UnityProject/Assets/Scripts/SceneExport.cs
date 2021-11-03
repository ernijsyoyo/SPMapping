using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

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
        /// Output path of the export
        /// </summary>
        private static string xmlOutputPath = null;
        /// <summary>
        /// Total number of channels occupied by audio streams
        /// </summary>
        public static int OccupiedChannelCount = 0;
        public bool test = false;
        // alternative private void Start(){ExportScene(FindObjectsOfType<SPMesh>()); }

        private void Update()
        {
            if (test)
            {
                test = false;
                ExportScene(FindObjectsOfType<SPMesh>());
            }


        }

        /// <summary>
        /// Initiates a scene export
        /// </summary>
        public void ExportScene(SPMesh[] meshes)
        {
            // Create arrays of all relevant objects 
            // e.g.


            // Setup XML doc
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.NewLineOnAttributes = true;

            // Declare header
            doc = new XDocument(
                new XDeclaration("1.0", "utf-8", "yes"),
                new XComment(" Mapped Environment for Indoor Navigation "),
                new XComment(" Scene exported at time: " + DateTime.Now.ToString("HH:mm:ss; dd.MM.yyyy "))
            );
            WriteRootNode(doc);

            // Open a dialog window for saving the output path
            xmlOutputPath = UnityEditor.EditorUtility.SaveFilePanel("Save scene as XML", "", "sceneName", "xml");
            XElement sceneNode = doc.Document.Root; // Get the root node

            // Write elements according to their type

            WriteMeshes(sceneNode, meshes);

            // Save the file
            doc.Save(xmlOutputPath);
            print("Scene exported at: " + xmlOutputPath);
        }

        private void WriteRootNode(XDocument doc)
        {
            doc.Add(new XElement("Root"));
        }

        /// <summary>
        /// Write meshes to the given XML Root node
        /// </summary>
        /// <param name="XmlRoot"></param>
        /// <param name="allmeshes"></param>
        private void WriteMeshes(XElement XmlRoot, SPMesh[] allmeshes)
        {
            if (allmeshes.Length < 1)
            {
                return;
            }
            if (XmlRoot == null)
            {
                print("XMLRoot is null");
            }
            XmlRoot.Document.Root.Add(new XComment("Meshes"));
            List<XElement> nodes = new List<XElement>();

            foreach (var mesh in allmeshes)
            {
                // Check if previously exported, compare via IDs


                var MeshNode = GetMeshNode(mesh);
                nodes.Add(MeshNode);


            }
        }

        /// <summary>
        /// Get an EIF formatted Mesh node
        /// </summary>
        /// <param name="mesh">Mesh for which to retrieve the formatted XML node</param>
        /// <param name="transform">Optional transform</param>
        /// <returns></returns>
        private XElement GetMeshNode(SPMesh mesh)
        {
            List<Vector3> GeometryVertices = mesh.GetVertices();
            List<int> trianglePos = mesh.getMeshFacesNoDupes(); //getTrianglesObject

            // Writes the root mesh node with a pivot point
            var MeshNode = new XElement("Mesh",
                                new XAttribute("id", "mesh:" + mesh.gameObject.name),
                                new XAttribute("pos", gameObject.transform.position),
                                new XAttribute("rot", gameObject.transform.rotation));

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





