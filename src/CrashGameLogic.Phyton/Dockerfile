# Use an official Python runtime as a parent image
FROM python:3.9-slim
WORKDIR /src

COPY ["CrashGameLogic.Phyton/app.py", "CrashGameLogic.Phyton/"]

# Set the working directory in the container
WORKDIR /src/CrashGameLogic.Phyton

# Copy the current directory contents into the container
COPY . .

# Install Flask
RUN pip install Flask

# Make port 5000 available to the world outside this container
EXPOSE 5000

# Run app.py when the container launches
CMD ["python", "app.py"]
