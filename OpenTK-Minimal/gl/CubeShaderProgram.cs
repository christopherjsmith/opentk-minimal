using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTK_Minimal.gl
{
    class CubeShaderProgram : ShaderProgram
    {
        public const string SHADER_IN_POSITION = "in_Position";
        public const string SHADER_IN_COLOUR = "in_Color";
        public const string SHADER_UNIFORM_PVM = "pvm";

        public CubeShaderProgram() : base(vertShader, fragShader)
        {
            base.BindUniform(SHADER_UNIFORM_PVM);

            base.CompileShaders();

            base.BindAttribute(SHADER_IN_POSITION);
            base.BindAttribute(SHADER_IN_COLOUR);

            base.LinkAndClean();
        }

        /* TODO: Load these from Files? */
        private static string vertShader = @"
            #version 440 core

            in  vec3 in_Position;
            in  vec3 in_Color;
            uniform  mat4 pvm;

            out vec3 ex_Color;

            void main(void)
            {
                gl_Position = pvm * vec4(in_Position, 1.0);
 
                ex_Color = in_Color;
            }
        ";

        private static string fragShader = @"
            #version 440 core

            in  vec3 ex_Color;
            out vec4 color;

            void main(void)
            {
                color = vec4(ex_Color, 1.0);
            }
        ";
    }
}
