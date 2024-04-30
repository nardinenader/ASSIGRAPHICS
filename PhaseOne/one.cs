
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Tao.OpenGl;
using GlmNet;
//include GLM library


using System.IO;

namespace Graphics
{
    class Renderer
    {
        Shader sh;
        uint vertexBufferID;

        //3D Drawing
        mat4 m;
        mat4 v;
        mat4 p;
        mat4 mvp;
        int MVP_ID;
        public void Initialize()
        {
            string projectPath = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName;
            sh = new Shader(projectPath + "\\Shaders\\SimpleVertexShader.vertexshader", projectPath + "\\Shaders\\SimpleFragmentShader.fragmentshader");
            Gl.glClearColor(1, 1, 1 , 1);
            float[] verts = { 
		       

                //screen
           


                0f , 0f , -1.8f, //0
                0 ,0,0, //b

                3.0f , 0f , -1.8f, //1
                0 , 0 , 0 ,//b

                3.0f , 3.0f , -1.8f, //2
                0,0,1, //b

                0f , 3.0f , -1.8f, //3
                0 ,0,1, //b

               

               

                //keyboard
                 
                0f , 0f , -2.0f ,//4
                0.55f ,0.55f,0.55f, //g

                0f , 0f , 0f ,//5
                0.55f ,0.55f,0.55f, //g


                3.0f , 0f , 0f ,//6
                0.55f ,0.55f,0.55f, //g



                 3.0f , 0f , -2.0f,  //7
                 0.55f ,0.55f,0.55f, //g



                //keys
                
                 0.4f , 0f , -2.0f ,//8
                 0.3f,0.3f,0.3f, //g2

                0.4f , 0f , 0f ,//9
                0.3f,0.3f,0.3f, //g2

                2.7f , 0f , 0f ,//10
                 0.3f,0.3f,0.3f, //g2


                 2.7f , 0f , -2.0f,  //11
                 0.3f,0.3f,0.3f, //g2




                //mouse
               -1.5f, 1f, -2.0f,  //12
               0 , 0 , 0 ,

             //cable
               -1.5f, 1f, -2.0f, //13
               0 , 0 , 0 ,
               -1.5f, 1f, 0f, //14
               0 , 0, 0,

              0f , 0f , -2.0f , //15

               0 , 0 , 0 ,
              -1.5f, 1f, 0f, //16
               0 , 0, 0,

               //outline
                0f , 0f , -1.8f, //17
                0 ,0,0, //b

                3.0f , 0f , -1.8f, //18
                0 , 0 , 0 ,

                3.0f , 3.0f , -1.8f, //19
                0,0,0,

                0f , 3.0f , -1.8f, //20
                0 ,0,0, //b


                //outline 2 
                 0f , 0f , -2.0f ,//21
                0,0,0, 

                0f , 0f , 0f ,//22
                0,0,0,


                3.0f , 0f , 0f ,//23
                 0,0,0,


                 3.0f , 0f , -2.0f,  //24
                 0,0,0,

		       
            };


            vertexBufferID = GPU.GenerateBuffer(verts);

            
            p = glm.perspective(45, 4 / 3.0f, 0.1f, 100);
            
            v = glm.lookAt(
                new vec3(3,5,5),// eye
                new vec3(0,2,0), // center
                new vec3(0,1,0)); // up
            
            m = new mat4(1);
            
            List<mat4> mvpList = new List<mat4>();
            mvpList.Add(m);
            mvpList.Add(v);
            mvpList.Add(p);
            mvp = MathHelper.MultiplyMatrices(mvpList);

            sh.UseShader();


            
            MVP_ID = Gl.glGetUniformLocation(sh.ID, "MVP");
            
            Gl.glUniformMatrix4fv(MVP_ID, 1, Gl.GL_FALSE, mvp.to_array());
        }

        public void Draw()
        {
            Gl.glClear(Gl.GL_COLOR_BUFFER_BIT);
            
            Gl.glEnableVertexAttribArray(0);
            Gl.glVertexAttribPointer(0, 3, Gl.GL_FLOAT, Gl.GL_FALSE, 6*sizeof(float), (IntPtr)0);
            Gl.glEnableVertexAttribArray(1);
            Gl.glVertexAttribPointer(1, 3, Gl.GL_FLOAT, Gl.GL_FALSE, 6 * sizeof(float), (IntPtr)(3*sizeof(float)));

            Gl.glDrawArrays(Gl.GL_QUADS, 0, 4);
            Gl.glDrawArrays(Gl.GL_QUADS, 4, 4);
            Gl.glDrawArrays(Gl.GL_QUADS, 8, 4);
            Gl.glPointSize(17);
            Gl.glDrawArrays(Gl.GL_POINTS, 12, 1);
            Gl.glDrawArrays(Gl.GL_LINE_LOOP, 13, 4);
            Gl.glDrawArrays(Gl.GL_LINE_LOOP, 17, 4);
            Gl.glDrawArrays(Gl.GL_LINE_LOOP, 21, 4);
          



            Gl.glDisableVertexAttribArray(0);
            Gl.glDisableVertexAttribArray(1);
        }
        public void Update()
        {
        }
        public void CleanUp()
        {
            sh.DestroyShader();
        }
    }
}
