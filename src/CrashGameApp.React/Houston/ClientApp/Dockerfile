# 1. Build Stage
FROM node:16-alpine as builder
WORKDIR /src

# Correct the package.json copy path
COPY CrashGameReact.App/package.json ./ 
RUN npm install

# Assuming CrashGameReact.App contains your React app, adjust if it's in a different location
COPY CrashGameReact.App ./
RUN npm run build

# 2. Run Stage
FROM nginx
EXPOSE 80

# If the build output is indeed in /src/build, this should be correct
COPY --from=builder /src/build /usr/share/nginx/html