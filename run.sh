#/bin/sh

download_and_run(){

    GIT_USERNAME="$1"
    GIT_REPOSITORY="$2"
    TAG="$3"

    echo "Downloading $GIT_USERNAME/$GIT_REPOSITORY with tag $TAG"

    tmp_dir=$(mktemp -d -t ci-XXXXXXXXXX)
    cd $tmp_dir

    case "$(uname -s)" in

    Darwin)
        echo 'Mac OS X'

        curl -s -L --output - "https://github.com/$GIT_USERNAME/$GIT_REPOSITORY/releases/download/$TAG/StandaloneOSX.zip" | bsdtar -xf-
        
        xattr -d com.apple.quarantine StandaloneOSX.app
        cd StandaloneOSX.app/Contents/MacOS
        chmod +x *
        find . -type f -exec "{}" +;

        ;;

    Linux)
        echo 'Linux'

        curl -s -L "https://github.com/$GIT_USERNAME/$GIT_REPOSITORY/releases/download/$TAG/StandaloneLinux64.zip" --output StandaloneLinux64.zip
        unzip StandaloneLinux64.zip
        rm StandaloneLinux64.zip
        chmod +x StandaloneLinux64
        ./StandaloneLinux64

        ;;

    CYGWIN*|MINGW32*|MSYS*|MINGW*)
        echo 'MS Windows'

        curl -s -L -k "https://github.com/$GIT_USERNAME/$GIT_REPOSITORY/releases/download/$TAG/StandaloneWindows.zip" --output StandaloneWindows.zip
        unzip StandaloneWindows.zip
        rm StandaloneWindows.zip
        ./StandaloneWindows.exe

        ;;

    *)
        echo 'Unsupported OS' 
        ;;
    esac

    rm -rf $tmp_dir

}

get_latest_release() {
  curl --silent "https://api.github.com/repos/$1/releases/latest" | # Get latest release from GitHub api
    grep '"tag_name":' |                                            # Get tag line
    sed -E 's/.*"([^"]+)".*/\1/'                                    # Pluck JSON value
}

GIT_USERNAME=humbertodias
GIT_REPOSITORY=unity-real-time-strategy-game
LATEST_TAG=$(get_latest_release $GIT_USERNAME/$GIT_REPOSITORY)

download_and_run $GIT_USERNAME $GIT_REPOSITORY $LATEST_TAG
