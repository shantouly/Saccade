import time
import RPi.GPIO as GPIO

GPIO.setmode(GPIO.BOARD) # 设置针脚编号模式为1～40顺序编号
GPIO.setwarnings(False)

########电机驱动接口定义#################
ENA = 13    # L298使能A
ENB = 15    # L298使能B
IN1 = 31    # 电机接口1
IN2 = 33    # 电机接口2
IN3 = 35    # 电机接口3
IN4 = 37    # 电机接口4

frequency = 30 # 高电平脉冲频率
dc = 30 # 占空比，高电平时间占比

#########电机初始化为LOW#################
GPIO.setup(ENA, GPIO.OUT, initial=GPIO.LOW)
ENA_pwm = GPIO.PWM(ENA, frequency)
ENA_pwm.start(0)
ENA_pwm.ChangeDutyCycle(dc)
GPIO.setup(IN1, GPIO.OUT, initial=GPIO.LOW)
GPIO.setup(IN2, GPIO.OUT, initial=GPIO.LOW)

GPIO.setup(ENB, GPIO.OUT, initial=GPIO.LOW)
ENB_pwm = GPIO.PWM(ENB, frequency)
ENB_pwm.start(0)
ENB_pwm.ChangeDutyCycle(dc)
GPIO.setup(IN3, GPIO.OUT, initial=GPIO.LOW)
GPIO.setup(IN4, GPIO.OUT, initial=GPIO.LOW)

def Motor_Forward():
    print( 'motor forward' )
    GPIO.output(ENA, True)
    GPIO.output(ENB, True)
    GPIO.output(IN1, False)
    GPIO.output(IN2, True)
    GPIO.output(IN3, False)
    GPIO.output(IN4, True)

def Motor_Backward():
    print( 'motor_backward' )
    GPIO.output(ENA, True)
    GPIO.output(ENB, True)
    GPIO.output(IN1, True)
    GPIO.output(IN2, False)
    GPIO.output(IN3, True)
    GPIO.output(IN4, False)

def Motor_TurnLeft():
    print( 'motor_turnleft' )
    GPIO.output(ENA, True)
    GPIO.output(ENB, True)
    GPIO.output(IN1, True)
    GPIO.output(IN2, False)
    GPIO.output(IN3, False)
    GPIO.output(IN4, True)

def Motor_TurnRight():
    print( 'motor_turnright' )
    GPIO.output(ENA, True)
    GPIO.output(ENB, True)
    GPIO.output(IN1, False)
    GPIO.output(IN2, True)
    GPIO.output(IN3, True)
    GPIO.output(IN4, False)

def Motor_Stop():
    print( 'motor_stop' )
    GPIO.output(ENA, False)
    GPIO.output(ENB, False)
    GPIO.output(IN1, False)
    GPIO.output(IN2, False)
    GPIO.output(IN3, False)
    GPIO.output(IN4, False)