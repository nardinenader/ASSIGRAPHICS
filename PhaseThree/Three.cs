Can't create, edit, or upload … Not enough storage. Get 100 GB of storage for EGP 14.99 EGP 0 for 1 month.
20201701909.cs
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Tao.OpenGl;
using GlmNet;
using System.IO;

namespace Graphics
{
    class Renderer
    {
        Shader sh;
        uint vertexBufferID;
        uint vertexBufferID2;
        int transID;
        int viewID;
        int projID;
        mat4 scaleMat;

        mat4 ProjectionMatrix;
        mat4 ViewMatrix;


        public Camera cam;

        Texture tex1;
        Texture tex2;
        Texture tex3;
        Texture tex4;
        Texture tex5;
        Texture tex6;
       

        public void Initialize()
        {
            string projectPath = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName;
            sh = new Shader(projectPath + "\\Shaders\\SimpleVertexShader.vertexshader", projectPath + "\\Shaders\\SimpleFragmentShader.fragmentshader");

            tex1 = new Texture(projectPath + "\\Textures\\windows.jpg", 1);
            tex2 = new Texture(projectPath + "\\Textures\\keyboard1.jpg", 2);
            tex3 = new Texture(projectPath + "\\Textures\\key.jpg", 3);
            tex4 = new Texture(projectPath + "\\Textures\\mouse.jpg", 4);
            tex5 = new Texture(projectPath + "\\Textures\\ltexture.jpg", 5);
            tex6 = new Texture(projectPath + "\\Textures\\table.jpg", 6);
            



            Gl.glClearColor(0.8f, 0.8f, 0.8f, 1);


            float[] verts = {

                //screen
                -0.5f, 0.5f, 0.0f, //0
                 1,1,1,
                 0,0,

                 0.5f, 0.5f, 0.0f, //1
                 1,1,1,
                 0,0,

                 0.5f,  -0.5f, 0.0f,//2
                 1,1,1,
                  0,0,

                 -0.5f,  -0.5f, 0.0f, //3
                  1,1,1,
                 0,0,


                 //window

                  -0.45f, 0.45f, 0.0f, //4
                  0,0,0,

                 0,1,

                 
                 0.45f, 0.45f, 0.0f, //5
                 0,0,0,

                 1,1,

                 0.45f,  -0.45f, 0.0f,//6
                  0,0,0,
                 1,0,

                 -0.45f,  -0.45f, 0.0f, //7
                 0,0,0,

                 0,0,


                 //cable

                0.45f, -0.55f , 0.0f,//8
                0 , 0 , 0 ,
               0,0,

               0.7f, -0.55f , 0.0f, //9
                0 , 0 , 0 ,
               0,0,

               0.7f, -0.7f , 0.0f, //10
                0 , 0 , 0 ,
               0,0,


               //mouse
                0.7f , -0.35f , 0.0f,  //11
                0.5f,0.5f,0.5f,
                0,0,

                0.8f, -0.35f , 0.0f,
                0.5f,0.5f,0.5f,
                0,1,

                0.8f , -0.75f , 0.3f,
                 0.5f,0.5f,0.5f,
                1,1,

                0.7f, -0.75f ,  0.3f,
                 0.5f,0.5f,0.5f,
                1,0,


                //keyboard
                -1f, -1f , 0.5f,//15
                 1,0,0,
                 0,0,

                 -0.5f,  -0.5f, 0.0f,
                 1,0,0,
                 0,1,

                  0.5f,  -0.5f, 0.0f,
                 1,0,0,
                 1,1,

                 0f , -1f, 0.5f,
                 1,0,0,
                 1,0,


                 //keys
                  -0.75f, -0.8f , 0.0f,//19
                1,0,0,
                0,1,

                -0.45f , -0.55f, 0.0f,
                1,0,0,
                0,0,

                0.2f , -0.55f, 0.0f,
                1,0,0,
                0,0,

                0.0f,-0.8f,  0.0f,
                1,0,0,
                0,1,


                

            };


            float[] ground = {

                -5.0f, -1.0f, 5.0f,//1
                 0,0,1,
                 0,0,

                 5.0f, 1.0f, -5.0f,//2
                 0,0,1,
                 1,1,

                  5.0f, -5.0f, 5.0f,
                 0,0,1,
                 0,1,

                -5.0f, -5.0f, -5.0f,
                 0,0,1,
                 1,0,

                

                

            };
           

           

            




            vertexBufferID = GPU.GenerateBuffer(verts);
            vertexBufferID2 = GPU.GenerateBuffer(ground);

            scaleMat = glm.scale(new mat4(1),new vec3(2f, 2f, 2.0f));

            cam = new Camera();

            ProjectionMatrix = cam.GetProjectionMatrix();
            ViewMatrix = cam.GetViewMatrix();

            transID = Gl.glGetUniformLocation(sh.ID, "model");
            projID = Gl.glGetUniformLocation(sh.ID, "projection");
            viewID = Gl.glGetUniformLocation(sh.ID, "view");

        }

        public void Draw()
        {
            Gl.glClear(Gl.GL_COLOR_BUFFER_BIT);
            sh.UseShader();

            Gl.glUniformMatrix4fv(transID, 1, Gl.GL_FALSE, scaleMat.to_array());
            Gl.glUniformMatrix4fv(projID, 1, Gl.GL_FALSE, ProjectionMatrix.to_array());
            Gl.glUniformMatrix4fv(viewID, 1, Gl.GL_FALSE, ViewMatrix.to_array());


            GPU.BindBuffer(vertexBufferID2);
            Gl.glEnableVertexAttribArray(0);
            Gl.glVertexAttribPointer(0, 3, Gl.GL_FLOAT, Gl.GL_FALSE, 8 * sizeof(float), IntPtr.Zero);
            Gl.glEnableVertexAttribArray(1);
            Gl.glVertexAttribPointer(1, 3, Gl.GL_FLOAT, Gl.GL_FALSE, 8 * sizeof(float), (IntPtr)(3 * sizeof(float)));
            Gl.glEnableVertexAttribArray(2);
            Gl.glVertexAttribPointer(2, 2, Gl.GL_FLOAT, Gl.GL_FALSE, 8 * sizeof(float), (IntPtr)(6 * sizeof(float)));

            tex6.Bind();
            Gl.glDrawArrays(Gl.GL_QUADS, 0, 4);
           



            GPU.BindBuffer(vertexBufferID);
            Gl.glEnableVertexAttribArray(0);
            Gl.glVertexAttribPointer(0, 3, Gl.GL_FLOAT, Gl.GL_FALSE, 8 * sizeof(float), IntPtr.Zero);
            Gl.glEnableVertexAttribArray(1);
            Gl.glVertexAttribPointer(1, 3, Gl.GL_FLOAT, Gl.GL_FALSE, 8 * sizeof(float), (IntPtr)(3 * sizeof(float)));
            Gl.glEnableVertexAttribArray(2);
            Gl.glVertexAttribPointer(2, 2, Gl.GL_FLOAT, Gl.GL_FALSE, 8 * sizeof(float), (IntPtr)(6 * sizeof(float)));

            tex5.Bind();
            Gl.glDrawArrays(Gl.GL_QUADS, 0, 4);
            tex1.Bind();
            Gl.glDrawArrays(Gl.GL_QUADS, 4, 4);
            tex5.Bind();
            Gl.glDrawArrays(Gl.GL_LINES, 8, 3);
            tex5.Bind();
            Gl.glDrawArrays(Gl.GL_QUADS, 11, 4);
            tex3.Bind();
            Gl.glDrawArrays(Gl.GL_QUADS, 15, 4);
            






            Gl.glDisableVertexAttribArray(0);
            Gl.glDisableVertexAttribArray(1);
            Gl.glDisableVertexAttribArray(2);
        }
        public void Update()
        {
            cam.UpdateViewMatrix();
            ProjectionMatrix = cam.GetProjectionMatrix();
            ViewMatrix = cam.GetViewMatrix();
        }
        public void CleanUp()
        {
            sh.DestroyShader();
        }
    }
}
