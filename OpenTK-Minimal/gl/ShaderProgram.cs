using System;
using System.Collections.Generic;

using OpenTK;
using OpenTK.Graphics.OpenGL4;

namespace OpenTK_Minimal
{
    /**
     * Shader programs should call "CompileShaders" bind any Attributes then call "LinkAndClean".
     * 
     */
    class ShaderProgram : GLObject
    {
        private string vertexShaderSrc;
        private string fragmentShaderSrc;
        
        private int programId;
        private int vertexShaderId;
        private int fragmentShaderId;

        private int attributeIdTrack = 0;

        private Dictionary<string, int> uniforms = new Dictionary<string, int>();
        private Dictionary<string, int> attribs = new Dictionary<string, int>();

        public ShaderProgram(string vertexShader, string fragmentShader)
        {
            this.vertexShaderSrc = vertexShader;
            this.fragmentShaderSrc = fragmentShader;
        }

        public void Destory()
        {
            /* Clean up any references */
            GL.DeleteProgram(this.programId);
        }
        
        public void Render()
        {
            /* Lets bind the shader */
            GL.UseProgram(this.programId);
        }

        public int BindUniform(string name)
        {
            int id = GL.GetUniformLocation(programId, name);

            uniforms.Add(name, id);

            return id;
        }

        public int BindAttribute(string name)
        {
            /* Use ID and set next */
            GL.BindAttribLocation(programId, attributeIdTrack, name);

            attribs.Add(name, attributeIdTrack);

            return attributeIdTrack++;
        }

        public int GetAttribute(string name)
        {
            attribs.TryGetValue(name, out int id);

            return id;
        }

        public void UniformMatrix4(string name, ref Matrix4 data)
        {
            uniforms.TryGetValue(name, out int id);
            GL.UniformMatrix4(0 , false, ref data);
        }

        public void CompileShaders()
        {
            /* Create a vertex shader, with given source */
            vertexShaderId = CreateShader(ShaderType.VertexShader, this.vertexShaderSrc);

            /* Create a fragment shader, with given source */
            fragmentShaderId = CreateShader(ShaderType.FragmentShader, this.fragmentShaderSrc);

            /* Create Shader Program & Bind Shaders */
            programId = GL.CreateProgram();
            GL.AttachShader(programId, vertexShaderId);
            GL.AttachShader(programId, fragmentShaderId);
        }

        public void LinkAndClean()
        {
            GL.LinkProgram(programId);

            /* Clean up temporary objects */
            GL.DetachShader(programId, vertexShaderId);
            GL.DetachShader(programId, fragmentShaderId);
            GL.DeleteShader(vertexShaderId);
            GL.DeleteShader(fragmentShaderId);
        }
        
        private int CreateShader(ShaderType type, string source)
        {
            int shaderId;

            /* Create a shader of the given type */
            shaderId = GL.CreateShader(type);

            /* Bind and compile source */
            GL.ShaderSource(shaderId, source);
            GL.CompileShader(shaderId);

            ValidateShader(shaderId);

            return shaderId;
        }

        private void ValidateShader(int shaderId)
        {
            /* TODO: Read the shader compilation logs and error out if something went wrong! */
        }
    }
}
