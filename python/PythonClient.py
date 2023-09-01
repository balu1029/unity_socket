import socket
import random

# Server address and port
server_address = "127.0.0.1"  # Change to the server's IP address
server_port = 12345  # Change to the server's port

# Generate a random coordinate
x = random.uniform(-10, 10)
y = random.uniform(0, 5)
z = random.uniform(-10, 10)

# Convert the coordinates to a string
coordinate = f"{x},{y},{z}"

# Create a socket object
client_socket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)

try:
    # Connect to the server
    client_socket.connect((server_address, server_port))
    print(f"Connected to {server_address}:{server_port}")

    # Send the coordinate to the server
    client_socket.sendall(coordinate.encode())

    # Receive and print the server's response (optional)
    response = client_socket.recv(1024).decode()
    print("Server response:", response)

except Exception as e:
    print("Error:", e)

finally:
    # Close the socket
    client_socket.close()
