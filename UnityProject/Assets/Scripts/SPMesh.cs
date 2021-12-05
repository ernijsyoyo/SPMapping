using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Linq;


namespace SP
{

    /// <summary>
    /// Class for specifying a custom Mesh entity
    /// </summary>

    public class SPMesh : MonoBehaviour
    {
        /// <summary>
        /// Denotes whether child objects should be included for exporting and mesh data gathering
        /// </summary>
        public bool includeChildObjects;

        /// <summary>
        /// Denotes whether data export should be done with duplicate vertex removal
        /// </summary>
        public bool exportWithoutDuplicateVertices = true;

        /// <summary>
        /// Return true if object has a MeshFilter (e.g. not blank game object)
        /// </summary>
        /// <returns></returns>
        public bool CheckIfAcousticEnvironmentCompatible()
        {
            return (gameObject.GetComponent<MeshFilter>() != null);
        }

        private Vector3[] verticesNoDupesArray = null;


        /// <summary>
        /// Get number of vertices on the object for statistical purposes
        /// </summary>
        /// <returns>Number of vertices of a whole entity (with or without children as specified by user) </returns>
        public int numberOfObjVertices()
        {
            // Get the number of vertices
            int numVertices = numbMeshVertices(gameObject);

            // Add the number of child vertices
            if (includeChildObjects == true)
            {
                var childMesh = GetComponentsInChildren<MeshFilter>();
                foreach (var objMesh in childMesh)
                {
                    if (objMesh.gameObject.GetComponent<SPMesh>() == null)
                    {
                        numVertices += numbMeshVertices(objMesh.gameObject);
                    }
                }
            }
            return numVertices;
        }
        /// <summary>
        /// Wrapper function for returning vertex count of a single game object
        /// </summary>
        /// <param name="unityGameObject"> Game object for which you wish to know the vertex count</param>
        /// <returns> Integer of mesh vertex count</returns>
        private int numbMeshVertices(GameObject unityGameObject)
        {
            var objMesh = unityGameObject.GetComponent<MeshFilter>();
            if (objMesh == null)
            {
                return 0;
            }
            return objMesh.sharedMesh.vertexCount;

        }

        /// <summary>
        /// Extracts triangles.count from the gameobject that this script is attached to (with or without children as specified by the user)
        /// </summary>
        /// <returns>Integer of number of faces</returns>
        public int numberOfObjTriangles()
        {
            // Get for this specific game object
            int numTriangles = numMeshTriangles(gameObject);

            // Get child faces
            if (includeChildObjects == true)
            {
                var childMesh = GetComponentsInChildren<MeshFilter>();
                foreach (var objMesh in childMesh)
                {
                    if (objMesh.gameObject.GetComponent<SPMesh>() == null)
                    {
                        numTriangles += numMeshTriangles(objMesh.gameObject);
                    }
                }
            }
            return numTriangles;
        }
        /// <summary>
        /// Get number of triangles per a given object from its MeshFilter component
        /// </summary>
        /// <param name="unityGameObject">Game object for which we wish to get the triangle count</param>
        /// <returns>Intger face count</returns>
        private int numMeshTriangles(GameObject unityGameObject)
        {
            var objMesh = unityGameObject.GetComponent<MeshFilter>();
            if (objMesh == null)
            {
                return 0;
            }
            int numMeshTriangles = objMesh.sharedMesh.triangles.Length;
            return numMeshTriangles / 3; // divide by 3 because the 1-D array consists of vertex indices
        }

        /// <summary>
        /// Get a list of vertices for this game object
        /// </summary>
        /// <param name="WithoutDuplicateVertices">With or without duplicate vertices</param>
        /// <returns>List of vec3 positions per vertex </returns>
        public List<Vector3> GetVertices()
        {
            if (exportWithoutDuplicateVertices)
                return getVertObjNoDupes();
            else
                return getVerticesObject();
        }

        /// <summary>
        /// Get a list of faces for this game object
        /// </summary>
        /// <param name="WithoutDuplicateVertices">With or without duplicate faces</param>
        /// <returns>List of ints</returns>
        public List<int> GetFaces()
        {
            if (exportWithoutDuplicateVertices)
                return getMeshFacesNoDupes();
            else
                return getTrianglesObject();
        }


        /* XML EXPORT DATA */

        /// <summary>
        /// Get a full list of vertices of a mesh entity with child object included if applicable
        /// </summary>
        /// <returns></returns>
        public List<Vector3> getVerticesObject()
        {
            // Get vertices for this game object
            List<Vector3> vertexPositions = mapVerticesMesh(gameObject);

            // Get vertices for children if applicable
            if (includeChildObjects == true)
            {
                var childMeshes = GetComponentsInChildren<MeshFilter>();
                foreach (var childMesh in childMeshes)
                {
                    // If a child has an SP Mesh, assume that it is intended and should be exported as a seperate mesh
                    if (childMesh.gameObject.GetComponent<SPMesh>() == null)
                    {
                        List<Vector3> childVertices = mapChildVertices(gameObject, childMesh.gameObject);
                        vertexPositions.AddRange(childVertices);
                    }
                }
            }
            return vertexPositions;
        }

        public void resetVerticesNoDupesArray()
        {
            verticesNoDupesArray = null;
        }

        /// <summary>
        /// Get vertices for a whole Mesh entity and remove duplicates
        /// </summary>
        /// <returns></returns>
        public List<Vector3> getVertObjNoDupes()
        {
            List<Vector3> allVertices = getVerticesObject();
            List<Vector3> verticesNoDupes = new List<Vector3>();

            if(verticesNoDupesArray != null)
            {
                return verticesNoDupesArray.ToList();
            }

            for (int i = 0; i < allVertices.Count; i++)
            {
                if(i % 1000 == 0) {
                    print("Fetching vertices.." + i + "/" + allVertices.Count);
                }
                
                if (!verticesNoDupes.Contains(allVertices[i]))
                {
                    verticesNoDupes.Add(allVertices[i]);
                }
            }
            verticesNoDupesArray = verticesNoDupes.ToArray();
            return verticesNoDupes;
        }

        /// <summary>
        /// Return mesh triangles with rearranged face indices for duplicate removed vertices
        /// </summary>
        /// <returns></returns>
        public List<int> getMeshFacesNoDupes()
        {
            /*
                * -Interate through each 
                * 
                * triangle v1-v2-v3 indexes 
                * Check if verticesNoDupes contains a vertex with index of allFaces[i]
                * if yes then determine which index is the initial value
                * set that index in facesReArranged
                * 
                * otherwise just copy the vertex and face index information from the full list 
                */

            List<Vector3> allVertices = getVerticesObject();
            List<int> allFaces = getTrianglesObject();
            List<Vector3> verticesNoDupes = getVertObjNoDupes();
            List<int> facesReArranged = new List<int>();

            for (int i = 0; i < allFaces.Count; i++)
            {
                if (i % 1000 == 0)
                {
                    print("Fetching faces.." + i + "/" + allFaces.Count);
                }
                for (int j = 0; j < verticesNoDupes.Count; j++)
                {

                    if (verticesNoDupes[j] == allVertices[allFaces[i]])
                    {
                        facesReArranged.Add(j);
                        break;
                    }
                }
            }
            return facesReArranged;
        }



        /// <summary>
        /// Adds scale multiply to the mesh vertices since EIF does not have a notion of scale for a mesh
        /// </summary>
        /// <param name="unityGameObject"></param>
        /// <returns></returns>
        public List<Vector3> mapVerticesMesh(GameObject unityGameObject)
        {
            MeshFilter mesh = unityGameObject.GetComponent<MeshFilter>();

            if (mesh == null)
            {
                return new List<Vector3>();
            }

            var meshList = mesh.sharedMesh.vertices.ToList();
            var scaledMeshList = new List<Vector3>();
            for (int i = 0; i < meshList.Count; i++)
            {
                scaledMeshList.Add(new Vector3(meshList[i].x * unityGameObject.transform.lossyScale.x,
                                                meshList[i].y * unityGameObject.transform.lossyScale.y,
                                                meshList[i].z * unityGameObject.transform.lossyScale.z));
            }
            return scaledMeshList;
        }

        /// <summary>
        /// Map the vertices of the child relative to its parent
        /// </summary>
        /// <param name="parent">Parent</param>
        /// <param name="child">Child</param>
        /// <returns>List of child's vertices</returns>
        public List<Vector3> mapChildVertices(GameObject parent, GameObject child)
        {
            var oldRot = parent.transform.rotation;
            parent.transform.rotation = new Quaternion(0, 0, 0, 1);
            var childLocalPos = child.transform.position - parent.transform.position;
            var parentScale = parent.transform.lossyScale;
            var childVertices = mapVerticesMesh(child);
            var childVertLocation = getVerticesMeshLocation(child, childVertices);
            List<Vector3> relativeChildVertices = new List<Vector3>();
            parent.transform.rotation = oldRot;

            // get child-to-parent relative by adding child local position displacement to vertex and multiply by parent scale
            foreach (var vertex in childVertLocation)
            {
                var relX = childLocalPos.x * parentScale.x + vertex.x;
                var relY = childLocalPos.y * parentScale.y + vertex.y;
                var relZ = childLocalPos.z * parentScale.z + vertex.z;
                Vector3 result = new Vector3(relX, relY, relZ);
                relativeChildVertices.Add(result);
            }
            return relativeChildVertices;
        }


        // Gives vertices X-Y-Z coordinates using gameobjects matrix transform
        public List<Vector3> getVerticesMeshLocation(GameObject unityGameObject, List<Vector3> VerticeListOfObject)
        {
            List<Vector3> verticesLocation = new List<Vector3>();
            Quaternion newRotation = new Quaternion();
            newRotation.eulerAngles = unityGameObject.transform.rotation.eulerAngles;//the degrees the vertices are to be rotated, for example (0,90,0)
            var center = new Vector3(0, 0, 0);

            for (int i = 0; i < VerticeListOfObject.Count; i++)
            {
                verticesLocation.Add(newRotation * (VerticeListOfObject[i] - center) + center);
            }
            return verticesLocation;
        }



        /// <summary>
        /// Get a list of face indices disregarding duplicate vertex removal
        /// </summary>
        /// <returns></returns>
        public List<int> getTrianglesObject()
        {
            /* - Retrieve parent's faces
                - Find all child objects with meshes
                - If no SPMesh, retrieve child triangle
                - Take each triangle value, current total vertice count
                - After each object's face is extracted, add triangle count to vertices of parent*/

            List<int> trianglesObject = getTrianglesMesh(gameObject);
            if (includeChildObjects == true)
            {
                int verticesOfParent = mapVerticesMesh(gameObject).Count;
                MeshFilter[] allChildMeshes = GetComponentsInChildren<MeshFilter>();

                foreach (var triangle in allChildMeshes)
                {
                    if (triangle.gameObject.GetComponent<SPMesh>() == null)
                    {
                        int[] childTriangles = triangle.sharedMesh.triangles;
                        for (int j = 0; j < childTriangles.Length; j++)
                        {
                            childTriangles[j] += verticesOfParent;
                        }
                        trianglesObject.AddRange(childTriangles);
                        verticesOfParent += mapVerticesMesh(triangle.gameObject).Count;
                    }
                }
            }
            return trianglesObject;
        }


        /// <summary>
        /// Get a list of triangles for the input mesh disregarding any duplicate vertex removal
        /// </summary>
        /// <param name="unityGameObject"></param>
        /// <returns></returns>
        private List<int> getTrianglesMesh(GameObject unityGameObject)
        {
            var mesh = unityGameObject.GetComponent<MeshFilter>();
            if (mesh == null)
            {
                return new List<int>();
            }
            return mesh.sharedMesh.triangles.ToList();
        }
    }
}


