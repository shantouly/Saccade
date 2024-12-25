import cv2
import socket
import threading
import struct
import time
import queue
from car import*

# 定义队列来存储待发送的帧
frame_queue = queue.Queue()

# 定义发送线程函数
def videoSendThread():
    while True:
        ret,frame = cap.read()
        if not ret:
            break
        frame_queue.put(frame)
        # 从队列中获取帧并发送
        if not frame_queue.empty():
            frame = frame_queue.get()
            # 编码图像
            result, encoded_frame = cv2.imencode('.jpg', frame, encode_param)

            # 发送图像数据大小
            size = len(encoded_frame)
            client_socket.sendall(struct.pack("L", size))

            # 发送图像数据
            client_socket.sendall(encoded_frame.tobytes())

# 定义接收线程函数
def receiveDataThread():
    while True:
        data = client_socket.recv(1024)
        if not data:
            break
        action = int(data)
        if action == 1:
            print("正在向前")
            Motor_Forward()
            
        elif action == 2:
            print("正在后退")
            Motor_Backward()
            
        elif action == 3:
            print("正在左转")
            Motor_TurnLeft()
            
        elif action == 4:
            print("正在右转")
            Motor_TurnRight()
           
        elif action == 5:
            print("已经停下")
            Motor_Stop()
            




# 创建 TCP 套接字并绑定到地址和端口
server_socket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
server_socket.bind(("192.168.8.6", 8877))
server_socket.listen()
print("监听中")

# 接受客户端连接
client_socket, ddr = server_socket.accept()
print('连接来自：', ddr)

# 打开摄像头
cap = cv2.VideoCapture(0)
encode_param = [int(cv2.IMWRITE_JPEG_QUALITY), 90]

# 创建发送和接收线程
send_thread = threading.Thread(target=videoSendThread)
receive_thread = threading.Thread(target=receiveDataThread)
send_thread.start()
receive_thread.start()









