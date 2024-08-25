#include "Uduino.h"  // Include Uduino library at the top of the sketch
Uduino uduino("IMU");

#include "I2Cdev.h"
#include "MPU6050_6Axis_MotionApps20.h"
#include "Wire.h"


MPU6050 mpu;

// MPU control/status vars
bool dmpReady = false;  // set true if DMP init was successful
uint8_t devStatus;      // return status after each device operation (0 = success, !0 = error)
uint16_t packetSize;    // expected DMP packet size (default is 42 bytes)
uint16_t fifoCount;     // count of all bytes currently in FIFO
uint8_t fifoBuffer[64]; // FIFO storage buffer

// orientation/motion vars
Quaternion q;           // [w, x, y, z]         quaternion container
//int16_t gyro[3];      // [x, y, z]            gyro sensor measurements
VectorInt16 gyro;       // [x, y, z]            gyro sensor measurements
VectorInt16 aa;         // [x, y, z]            accel sensor measurements
VectorInt16 aaReal;     // [x, y, z]            gravity-free accel sensor measurements
VectorInt16 aaWorld;    // [x, y, z]            world-frame accel sensor measurements
VectorFloat gravity;    // [x, y, z]            gravity vector
float euler[3];         // [psi, theta, phi]    Euler angle container
float ypr[3];           // [yaw, pitch, roll]   yaw/pitch/roll container and gravity vector
int16_t ax, ay, az;
int16_t gx, gy, gz;

#define LED_PIN 13
bool blinkState = false;

void setup() {
  Wire.begin();
  Wire.setClock(400000); // 400kHz I2C clock. Comment this line if having compilation difficulties

  Serial.begin(38400);

  while (!Serial); // wait for Leonardo enumeration, others continue immediately

  mpu.initialize();
  devStatus = mpu.dmpInitialize();
  mpu.setXGyroOffset(54); //++
  mpu.setYGyroOffset(-21); //--
  mpu.setZGyroOffset(5);

  if (devStatus == 0) {
    mpu.setDMPEnabled(true);
    // set our DMP Ready flag so the main loop() function knows it's okay to use it
    dmpReady = true;
    // get expected DMP packet size for later comparison
    packetSize = mpu.dmpGetFIFOPacketSize();
  } else {
    // Error
    Serial.println("Error!");
  }

   // configure Arduino LED for
   pinMode(LED_PIN, OUTPUT);
}



void loop() {
  uduino.update();

  if (uduino.isInit()) {
    if (!dmpReady) {
      Serial.println("IMU not connected.");
      delay(10);
      return;
    }

    int  mpuIntStatus = mpu.getIntStatus();
    fifoCount = mpu.getFIFOCount();

    if ((mpuIntStatus & 0x10) || fifoCount == 1024) { // check if overflow
      mpu.resetFIFO();
    } else if (mpuIntStatus & 0x02) {
      while (fifoCount < packetSize) fifoCount = mpu.getFIFOCount();

      mpu.getFIFOBytes(fifoBuffer, packetSize);
      fifoCount -= packetSize;

      //Sendmes();
      SendQuaternion();
      //SendGyro();
      
      //SendEuler();
      //SendYawPitchRoll();
      //SendRealAccel();
      //SendWorldAccel();

      // blink LED to indicate activity
      blinkState = !blinkState;
      digitalWrite(LED_PIN, blinkState);
    }
  }
}

void Sendmes(){
  mpu.dmpGetQuaternion(&q, fifoBuffer);
  Serial.print("r/");
  Serial.print(q.w, 4); Serial.print("/");
  Serial.print(q.x, 4); Serial.print("/");
  Serial.print(q.y, 4); Serial.print("/");
  Serial.print(q.z, 4); Serial.print("/");
      
  // read raw accel/gyro measurements from device
  mpu.getMotion6(&ax, &ay, &az, &gx, &gy, &gz);
  
  // display tab-separated accel/gyro x/y/z values
  Serial.print(ax); Serial.print("/");
  Serial.print(ay); Serial.print("/");
  Serial.print(az); Serial.print("/");
  Serial.print(gx); Serial.print("/");
  Serial.print(gy); Serial.print("/");
  Serial.println(gz);

}



void SendGyro() {
  mpu.dmpGetGyro(&gyro, fifoBuffer);
  Serial.print(gyro.x); Serial.print("/");
  Serial.print(gyro.y); Serial.print("/");
  Serial.println(gyro.z); 
//  gyro[0] = (fifoBuffer[16] << 8) + fifoBuffer[17];
//  gyro[1] = (fifoBuffer[20] << 8) + fifoBuffer[21];
//  gyro[2] = (fifoBuffer[24] << 8) + fifoBuffer[25];
//  Serial.print("/");
//  Serial.print(gyro[0]); Serial.print("/");
//  Serial.print(gyro[1]); Serial.print("/");
//  Serial.print(gyro[2]); 
}
 

void SendQuaternion() {
  mpu.dmpGetQuaternion(&q, fifoBuffer);
  Serial.print("r/");
  Serial.print(q.w, 4); Serial.print("/");
  Serial.print(q.x, 4); Serial.print("/");
  Serial.print(q.y, 4); Serial.print("/");
  Serial.println(q.z, 4);
}

void SendEuler() {
  // display Euler angles in degrees
  mpu.dmpGetQuaternion(&q, fifoBuffer);
  mpu.dmpGetEuler(euler, &q);
  Serial.print(euler[0] * 180 / M_PI); Serial.print("/");
  Serial.print(euler[1] * 180 / M_PI); Serial.print("/");
  Serial.println(euler[2] * 180 / M_PI);
}

void SendYawPitchRoll() {
  // display Euler angles in degrees
  mpu.dmpGetQuaternion(&q, fifoBuffer);
  mpu.dmpGetGravity(&gravity, &q);
  mpu.dmpGetYawPitchRoll(ypr, &q, &gravity);
  Serial.print(ypr[0] * 180 / M_PI); Serial.print("/");
  Serial.print(ypr[1] * 180 / M_PI); Serial.print("/");
  Serial.println(ypr[2] * 180 / M_PI);
}

void SendRealAccel() {
  // display real acceleration, adjusted to remove gravity
  mpu.dmpGetQuaternion(&q, fifoBuffer);
  mpu.dmpGetAccel(&aa, fifoBuffer);
  mpu.dmpGetGravity(&gravity, &q);
  mpu.dmpGetLinearAccel(&aaReal, &aa, &gravity);
  Serial.print("a/");
  Serial.print(aaReal.x); Serial.print("/");
  Serial.print(aaReal.y); Serial.print("/");
  Serial.println(aaReal.z);
}

void SendWorldAccel() {
  // display initial world-frame acceleration, adjusted to remove gravity
  // and rotated based on known orientation from quaternion
  mpu.dmpGetQuaternion(&q, fifoBuffer);
  mpu.dmpGetAccel(&aa, fifoBuffer);
  mpu.dmpGetGravity(&gravity, &q);
  mpu.dmpGetLinearAccel(&aaReal, &aa, &gravity);
  mpu.dmpGetLinearAccelInWorld(&aaWorld, &aaReal, &q);
  Serial.print("a/");
  Serial.print(aaWorld.x); Serial.print("/");
  Serial.print(aaWorld.y); Serial.print("/");
  Serial.println(aaWorld.z);
}
