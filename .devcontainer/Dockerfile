# Use Ubuntu as base OS
FROM ubuntu:24.04

# Install tools
RUN apt-get update
RUN apt-get install -y procps curl
RUN apt-get install wget -y

# Install .Net 8.0 and 9.0 SDK
RUN wget https://packages.microsoft.com/config/ubuntu/20.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
RUN dpkg -i packages-microsoft-prod.deb
RUN rm packages-microsoft-prod.deb
RUN apt-get update
RUN apt-get install -y dotnet-sdk-8.0

# Get and Install Node
# https://github.com/nodejs/node
RUN curl -sL https://deb.nodesource.com/setup_20.x | bash -
RUN apt-get update
RUN apt-get install -y nodejs

# Install Git
RUN apt-get install -y git
RUN apt-get install git-lfs

# Install GitHub CLI
# https://github.com/cli/cli
RUN curl -fsSL https://cli.github.com/packages/githubcli-archive-keyring.gpg | dd of=/etc/apt/trusted.gpg.d/githubcli-archive-keyring.gpg
RUN echo "deb [arch=$(dpkg --print-architecture) signed-by=/etc/apt/trusted.gpg.d/githubcli-archive-keyring.gpg] https://cli.github.com/packages stable main" | tee /etc/apt/sources.list.d/github-cli.list >/dev/null
RUN apt-get update
RUN apt-get install gh

# Install Shell
RUN mkdir install
COPY /shell /install/shell
RUN chmod +x /install/shell/install.sh
RUN /bin/bash -c "./install/shell/install.sh"

RUN dotnet tool update -g docfx