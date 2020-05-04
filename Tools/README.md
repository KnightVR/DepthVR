# Librealsense setup instrctions for Unity Android

Adapted from instrctions from [here](https://github.com/GeorgeAdamon/quest-realsense)

# Setup
[Download librealsense](https://github.com/IntelRealSense/librealsense.git)
```
git clone https://github.com/IntelRealSense/librealsense.git
```
[Download android studio](https://developer.android.com/studio)

Download Android 4.4 SDK and NDK inside Android Studio
Android Studio -> Settings -> System Settings -> Android SDK -> SDK Platforms -> Tick 'Android 4.4' -> SDK Tools -> Tick 'NDK (Side by side'

Add NDK to project:
Android Studio -> project structure -> Android NDK location

Close Android Studio (Closing Android Studio during the build setup will stop gradle and the build will fail)

# Build
Open command prompt As Admin
```
cd <librealsense_root_dir>\wrappers\android
gradlew assembleRelease
```

If the build is succesful, the generated .aar file will be located in <librealsense_root_dir>/wrappers/android/librealsense/build/outputs/aar