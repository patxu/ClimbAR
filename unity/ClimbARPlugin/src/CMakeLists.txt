find_package( OpenCV )

include_directories( ${OpenCV_INCLUDE_DIRS} )

add_executable( ClimbARPlugin MACOSX_BUNDLE Plugin.cpp)

target_link_libraries( ClimbARPlugin ${OpenCV_LIBS} )

add_custom_target (RenameExtension ALL COMMAND mv ${CMAKE_RUNTIME_OUTPUT_DIRECTORY}/ClimbARPlugin.app ${CMAKE_RUNTIME_OUTPUT_DIRECTORY}/ClimbARPlugin.bundle)
