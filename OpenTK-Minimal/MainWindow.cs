using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Input;
using OpenTK_Minimal.gl;

namespace OpenTK_Minimal
{
    public sealed class MainWindow : GameWindow
    {
        CubeShaderProgram shaderProgram;
        GLCube glCube;

        List<GLObject> renderList = new List<GLObject> {};

        public MainWindow() : base(1280,
                                    720,
                                    GraphicsMode.Default,
                                    "",
                                    GameWindowFlags.Default,
                                    DisplayDevice.Default,
                                    4, // OpenGL major version
                                    0, // OpenGL minor version
                                    GraphicsContextFlags.ForwardCompatible)
        { }

        protected override void OnLoad(EventArgs e)
        {
            CursorVisible = true;
            
            /* Set-up our basic Shader Program */
            shaderProgram = new CubeShaderProgram();

            /* Create a cube */
            glCube = new GLCube(ref shaderProgram);

            /* Set-up a render list */
            renderList.Add(shaderProgram);
            renderList.Add(glCube);

            /* Tag our close handler onto the event listener */
            Closed += OnClosed;
        }

        private void OnClosed(object sender, EventArgs eventArgs)
        {
            Exit();
        }

        public override void Exit()
        {
            /* Clean-up */
            renderList.ForEach(obj => obj.Destory());

            base.Exit();
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            /* Define title with FPS */
            Title = $"Refresh: (Vsync: {VSync}) FPS: {1f / e.Time:0}";
            
            /* Clear back colour, and enable depth testing */
            GL.ClearColor(Color4.CornflowerBlue);
            GL.Enable(EnableCap.DepthTest);
            GL.DepthFunc(DepthFunction.Less);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            /* Compute PVM Matrix, with inverse ordering for OpenTK */
            Matrix4 viewMatrix = Matrix4.Identity * Matrix4.LookAt(new Vector3(4, 3, 3), new Vector3(0, 0, 0), new Vector3(0, 1, 0)) * Matrix4.CreatePerspectiveFieldOfView((float)Math.PI / 4, 1280f / 720f, 0.1f, 100f);

            /* Bind the view matrix to the shader */
            shaderProgram.UniformMatrix4(CubeShaderProgram.SHADER_UNIFORM_PVM, ref viewMatrix);

            /* Render the list -- shader is first thing to "render" */
            renderList.ForEach(obj => obj.Render());

            /* Everything is rasterised, throw it up on screen */
            SwapBuffers();
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            HandleKeyboard();

            /* Business Logic here... */
        }

        private void HandleKeyboard()
        {
            var keyState = Keyboard.GetState();

            if (keyState.IsKeyDown(Key.Escape))
            {
                Exit();
            }
        }

        protected override void OnResize(EventArgs e)
        {
            /* Setup the corretc aspect ratio for new window size. */
            GL.Viewport(0, 0, Width, Height);
        }
    }
}
