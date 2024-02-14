FROM python:3.11
WORKDIR /code
COPY requirements.txt .
RUN pip3 install -r requirements.txt
# Install .NET 7
RUN apt-get update \
    && apt-get install -y --no-install-recommends \
        apt-transport-https \
        dirmngr \
        gnupg \
        ca-certificates \
    && wget https://packages.microsoft.com/config/debian/10/packages-microsoft-prod.deb -O packages-microsoft-prod.deb \
    && dpkg -i packages-microsoft-prod.deb \
    && apt-get update \
    && apt-get install -y dotnet-sdk-7.0
COPY . .
EXPOSE 443
ENTRYPOINT ["uvicorn", "app.main:app", "--host", "0.0.0.0", "--port", "443"]
