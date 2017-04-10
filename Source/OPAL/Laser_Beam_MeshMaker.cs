using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;   // Always needed     // Material/Graphics handling functions are found here
using RimWorld;      // RimWorld specific functions are found here
using Verse;         // RimWorld universal objects are here
//using Verse.AI;    // Needed when you do something with the AI
using Verse.Sound;   // Needed when you do something with the Sound
using Verse.Noise;

namespace RD_OPAL
{
    public static class Laser_Beam_MeshMaker
	{
        private static Mesh laserBeamMesh = null;

        private const float LightningHeight = 200f;
        private const float LightningRootXVar = 50f;
        private const float VertexInterval = 0.25f;
        public const float MeshWidth = 10f;
        private const float UVIntervalY = 0.04f;
        private const float PerturbAmp = 0.0f;
        private const float PerturbFreq = 0.007f;
        private static List<Vector2> verts2D;
        private static Vector2 lightningTop;

        public static Mesh NewBoltMesh()
        {
            if (laserBeamMesh == null)
            {
                Laser_Beam_MeshMaker.lightningTop = new Vector2(0f, LightningHeight);
                Laser_Beam_MeshMaker.MakeVerticesBase();
                Laser_Beam_MeshMaker.PeturbVerticesRandomly();
                Laser_Beam_MeshMaker.DoubleVertices();
                laserBeamMesh = Laser_Beam_MeshMaker.MeshFromVerts();
            }
            return laserBeamMesh;
        }
        private static void MakeVerticesBase()
        {
            int num = (int)Math.Ceiling((double)((Vector2.zero - Laser_Beam_MeshMaker.lightningTop).magnitude / VertexInterval));
            Vector2 b = Laser_Beam_MeshMaker.lightningTop / (float)num;
            Laser_Beam_MeshMaker.verts2D = new List<Vector2>();
            Vector2 vector = Vector2.zero;
            for (int i = 0; i < num; i++)
            {
                Laser_Beam_MeshMaker.verts2D.Add(vector);
                vector += b;
            }
        }
        private static void PeturbVerticesRandomly()
        {
            Perlin perlin = new Perlin(PerturbFreq, 2.0, 0.5, 6, Rand.Range(0, 2147483647), QualityMode.High);
            List<Vector2> list = Laser_Beam_MeshMaker.verts2D.ListFullCopy<Vector2>();
            Laser_Beam_MeshMaker.verts2D.Clear();
            int num = 0;
            foreach (Vector2 current in list)
            {
                float d = PerturbAmp * (float)perlin.GetValue((double)num, 0.0, 0.0);
                Vector2 item = current + d * Vector2.right;
                Laser_Beam_MeshMaker.verts2D.Add(item);
                num++;
            }
        }
        private static void DoubleVertices()
        {
            List<Vector2> list = Laser_Beam_MeshMaker.verts2D.ListFullCopy<Vector2>();
            Vector3 vector = default(Vector3);
            Vector2 a = default(Vector2);
            Laser_Beam_MeshMaker.verts2D.Clear();
            for (int i = 0; i < list.Count; i++)
            {
                if (i <= list.Count - 2)
                {
                    vector = Quaternion.AngleAxis(90f, Vector3.up) * (list[i] - list[i + 1]);
                    a = new Vector2(vector.y, vector.z);
                    a.Normalize();
					a.Scale(new Vector2(MeshWidth, 1f));
                }
                Vector2 item = list[i] - 1f * a;
                Vector2 item2 = list[i] + 1f * a;
                Laser_Beam_MeshMaker.verts2D.Add(item);
                Laser_Beam_MeshMaker.verts2D.Add(item2);
            }
        }
        private static Mesh MeshFromVerts()
        {
            Vector3[] array = new Vector3[Laser_Beam_MeshMaker.verts2D.Count];
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = new Vector3(Laser_Beam_MeshMaker.verts2D[i].x, 0f, Laser_Beam_MeshMaker.verts2D[i].y);
            }
            float num = 0f;
            Vector2[] array2 = new Vector2[Laser_Beam_MeshMaker.verts2D.Count];
            for (int j = 0; j < Laser_Beam_MeshMaker.verts2D.Count; j += 2)
            {
                array2[j] = new Vector2(0f, num);
                array2[j + 1] = new Vector2(1f, num);
                num += UVIntervalY;
            }
            int[] array3 = new int[Laser_Beam_MeshMaker.verts2D.Count * 3];
            for (int k = 0; k < Laser_Beam_MeshMaker.verts2D.Count - 2; k += 2)
            {
                int num2 = k * 3;
                array3[num2] = k;
                array3[num2 + 1] = k + 1;
                array3[num2 + 2] = k + 2;
                array3[num2 + 3] = k + 2;
                array3[num2 + 4] = k + 1;
                array3[num2 + 5] = k + 3;
            }
            return new Mesh
            {
                vertices = array,
                uv = array2,
                triangles = array3
            };
        }
    }
}
