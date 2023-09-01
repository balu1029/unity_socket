using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

public class TCPServer : MonoBehaviour
{
    private TcpListener server;
    private Thread listenerThread;
    private bool isRunning = true;

    // Port to listen on
    public int port = 12345;

    // Reference to the GameObject to move
    public Transform objectToMove;

    private void Start()
    {
        // Start the listener thread
        listenerThread = new Thread(StartListening);
        listenerThread.Start();
    }

    private void StartListening()
    {
        try
        {
            // Initialize the server
            server = new TcpListener(IPAddress.Any, port);
            server.Start();

            Debug.Log("Server is listening on port " + port);

            while (isRunning)
            {
                // Wait for a client to connect
                TcpClient client = server.AcceptTcpClient();
                Debug.Log("Client connected");

                // Handle client communication on a separate thread
                Thread clientThread = new Thread(() => HandleClient(client));
                clientThread.Start();
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Error: " + e.Message);
        }
    }

    private void HandleClient(TcpClient client)
    {
        try
        {
            // Get the client's stream for reading and writing
            NetworkStream stream = client.GetStream();
            byte[] buffer = new byte[1024];
            int bytesRead;

            while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0)
            {
                // Handle incoming data here
                string message = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                Debug.Log("Received message from client: " + message);

                // Parse the message as a coordinate (assuming format "x,y,z")
                string[] coordinates = message.Split(',');
                if (coordinates.Length == 3)
                {
                    float x = float.Parse(coordinates[0]);
                    float y = float.Parse(coordinates[1]);
                    float z = float.Parse(coordinates[2]);

                    // Move the object to the specified position
                    objectToMove.position = new Vector3(x, y, z);
                    Debug.Log("Moved object to position: " + objectToMove.position);
                }

                // Send a response back to the client (optional)
                string response = "Server received: " + message;
                byte[] responseBuffer = Encoding.ASCII.GetBytes(response);
                stream.Write(responseBuffer, 0, responseBuffer.Length);
            }

            // Close the client connection
            client.Close();
            Debug.Log("Client disconnected");
        }
        catch (Exception e)
        {
            Debug.LogError("Error handling client: " + e.Message);
        }
    }

    private void OnApplicationQuit()
    {
        // Stop listening and close the server when the application quits
        isRunning = false;
        if (server != null)
        {
            server.Stop();
        }

        if (listenerThread != null && listenerThread.IsAlive)
        {
            listenerThread.Join();
        }
    }
}
