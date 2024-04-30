
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Tao.OpenGl;

//include GLM library
using GlmNet;


using System.IO;
using System.Diagnostics;

namespace Graphics
{
    class Renderer
    {
        Shader sh;
        
        uint triangleBufferID;
        uint xyzAxesBufferID;

        //3D Drawing
        mat4 ModelMatrix;
        mat4 ViewMatrix;
        mat4 ProjectionMatrix;
        
        int ShaderModelMatrixID;
        int ShaderViewMatrixID;
        int ShaderProjectionMatrixID;

        const float rotationSpeed = 1f;
        float rotationAngle = 0;

        public float translationX=0, 
                     translationY=0, 
                     translationZ=0;

        Stopwatch timer = Stopwatch.StartNew();

        vec3 triangleCenter;

        public void Initialize()
        {
            string projectPath = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName;
            sh = new Shader(projectPath + "\\Shaders\\SimpleVertexShader.vertexshader", projectPath + "\\Shaders\\SimpleFragmentShader.fragmentshader");
            Gl.glClearColor(1, 1, 1, 1);
            
            float[] triangleVertices= {
            //screen
           


                0f , 0f , -21.0f, //0
                0 ,0,0, //b

                35.0f , 0f , -21.0f,//1
                0 , 0 , 0 ,

                 35.0f , 35.0f , -21.0f, //2
                0,0,1,

                0f , 35.0f , -21.0f ,//3
                0 ,0,1, //b

               

               

                //keyboard
                 0f , 35.0f , -21.0f ,//4
                0.8f ,0.8f,0.8f, //b

                0f , 35.0f , 0f ,//5
                0.8f ,0.8f,0.8f, //b


                35.0f , 35.0f , 0f, //6
                0.8f,0.8f,0.8f,


                35.0f , 35.0f , -21.0f, //7
                0.8f,0.8f,0.8f,


                //keys
                 4.9f , 35.0f , -21.0f ,//8
                0.5f,0.5f,0.5f, //b

                4.9f , 35.0f , -7.0f ,//9
                0.5f,0.5f,0.5f, //b

                30.1f , 35.0f , -7.0f, //10
                0.5f,0.5f,0.5f, //b


                30.1f , 35.0f , -21.0f, //11
                0.5f,0.5f,0.5f, //b


                //mouse
               21.0f, 35.0f, 21.0f,  //12
               0 , 0 , 0 ,


             //cable
               22.4f, 35.0f, 21.0f, //13
               0 , 0 , 0 ,
               22.4f , 35.0f , 0f, //14
               0 , 0, 0,

                22.4f, 35.0f, 0f, //15
               0 , 0 , 0 ,
               22.4f , 35.0f , 21.0f, //16
               0 , 0, 0
            }; // Triangle Center = (10, 7, -5)
            
            triangleCenter = new vec3(10, 7, -5);

            float[] xyzAxesVertices = {
		        //x
		        0.0f, 0.0f, 0.0f, 1.0f, 0.0f, 0.0f, //R
		        100.0f, 0.0f, 0.0f, 1.0f, 0.0f, 0.0f, //R
		        //y
	            0.0f, 0.0f, 0.0f, 0.0f, 1.0f, 0.0f, //G
		        0.0f, 100.0f, 0.0f, 0.0f, 1.0f, 0.0f, //G
		        //z
	            0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 1.0f,  //B
		        0.0f, 0.0f, -100.0f, 0.0f, 0.0f, 1.0f,  //B
            };


            triangleBufferID = GPU.GenerateBuffer(triangleVertices);
            xyzAxesBufferID = GPU.GenerateBuffer(xyzAxesVertices);

            
            ViewMatrix = glm.lookAt(
                        new vec3(50, 50, 50), 
                        new vec3(0, 0, 0), 
                        new vec3(0, 1, 0)  
                );
           
            ModelMatrix = new mat4(1);

            
            ProjectionMatrix = glm.perspective(45.0f, 4.0f / 3.0f, 0.1f, 100.0f);
           
            sh.UseShader();


            
            ShaderModelMatrixID = Gl.glGetUniformLocation(sh.ID, "modelMatrix");
            ShaderViewMatrixID = Gl.glGetUniformLocation(sh.ID, "viewMatrix");
            ShaderProjectionMatrixID = Gl.glGetUniformLocation(sh.ID, "projectionMatrix");

            Gl.glUniformMatrix4fv(ShaderViewMatrixID, 1, Gl.GL_FALSE, ViewMatrix.to_array());
            Gl.glUniformMatrix4fv(ShaderProjectionMatrixID, 1, Gl.GL_FALSE, ProjectionMatrix.to_array());

            timer.Start();
        }

        public void Draw()
        {
            sh.UseShader();
            Gl.glClear(Gl.GL_COLOR_BUFFER_BIT);
            sh.UseShader();
            Gl.glClear(Gl.GL_COLOR_BUFFER_BIT);
            Gl.glEnableVertexAttribArray(0);
            Gl.glVertexAttribPointer(0, 3, Gl.GL_FLOAT, Gl.GL_FALSE, 6 * sizeof(float), (IntPtr)0);
            Gl.glEnableVertexAttribArray(1);
            Gl.glVertexAttribPointer(1, 3, Gl.GL_FLOAT, Gl.GL_FALSE, 6 * sizeof(float), (IntPtr)(3 * sizeof(float)));

            Gl.glDrawArrays(Gl.GL_QUADS, 0, 4);
            Gl.glDrawArrays(Gl.GL_QUADS, 4, 4);
            Gl.glDrawArrays(Gl.GL_QUADS, 8, 4);
            Gl.glPointSize(15);
            Gl.glDrawArrays(Gl.GL_POINTS, 12, 1);
            Gl.glDrawArrays(Gl.GL_LINE_LOOP, 13, 4);

            #region XYZ axis
            Gl.glBindBuffer(Gl.GL_ARRAY_BUFFER, xyzAxesBufferID);

            Gl.glUniformMatrix4fv(ShaderModelMatrixID, 1, Gl.GL_FALSE, new mat4(1).to_array()); // Identity

            Gl.glEnableVertexAttribArray(0);
            Gl.glVertexAttribPointer(0, 3, Gl.GL_FLOAT, Gl.GL_FALSE, 6 * sizeof(float), (IntPtr)0);
            Gl.glEnableVertexAttribArray(1);
            Gl.glVertexAttribPointer(1, 3, Gl.GL_FLOAT, Gl.GL_FALSE, 6 * sizeof(float), (IntPtr)(3 * sizeof(float)));
             
            Gl.glDrawArrays(Gl.GL_LINES, 0, 6);

            Gl.glDisableVertexAttribArray(0);
            Gl.glDisableVertexAttribArray(1);

            #endregion

            #region Animated Triangle
            Gl.glBindBuffer(Gl.GL_ARRAY_BUFFER, triangleBufferID);

            Gl.glUniformMatrix4fv(ShaderModelMatrixID, 1, Gl.GL_FALSE, ModelMatrix.to_array());

            Gl.glEnableVertexAttribArray(0);
            Gl.glVertexAttribPointer(0, 3, Gl.GL_FLOAT, Gl.GL_FALSE, 6 * sizeof(float), (IntPtr)0);
            Gl.glEnableVertexAttribArray(1);
            Gl.glVertexAttribPointer(1, 3, Gl.GL_FLOAT, Gl.GL_FALSE, 6 * sizeof(float), (IntPtr)(3 * sizeof(float)));

            Gl.glDrawArrays(Gl.GL_TRIANGLES, 0, 3);

            Gl.glDisableVertexAttribArray(0);
            Gl.glDisableVertexAttribArray(1);
            #endregion
        }
        
        public void Update()
        {
            timer.Stop();
            var deltaTime = timer.ElapsedMilliseconds/1000.0f;
            rotationAngle += deltaTime * rotationSpeed;

            List<mat4> transformations = new List<mat4>();
            transformations.Add(glm.translate(new mat4(1), -1 * triangleCenter));
            transformations.Add(glm.rotate(rotationAngle, new vec3(0, 0, 1)));
            transformations.Add(glm.translate(new mat4(1),  triangleCenter));
            transformations.Add(glm.translate(new mat4(1), new vec3(translationX, translationY, translationZ)));

            ModelMatrix =  MathHelper.MultiplyMatrices(transformations);
            
            timer.Reset();
            timer.Start();
        }
        
        public void CleanUp()
        {
            sh.DestroyShader();
        }
    }
}
