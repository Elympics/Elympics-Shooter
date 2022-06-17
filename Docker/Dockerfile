ARG UNITY_VERSION

FROM registry.app.daftmobile.com/unity/unity-docker-build:${UNITY_VERSION}-web

WORKDIR /root/project
CMD ["/bin/bash", "-l"]

COPY Gemfile Gemfile.lock ./

RUN source /etc/profile.d/rvm.sh &&\
    bundle install &&\
    rm Gemfile Gemfile.lock &&\
    apt-get update &&\
    apt-get install ffmpeg python python-pip python-brotli -y &&\
    rm -rf /var/lib/apt/lists/* &&\
    rm -rf /tmp/*
