# Step 1: Build the Angular App
FROM node:18 AS build
WORKDIR /app
COPY package.json package-lock.json ./
RUN npm install
COPY . .
RUN npm run build --configuration=production

# Step 2: Serve App Using Nginx
FROM nginx:alpine
COPY --from=build /app/dist/weather-prediction /usr/share/nginx/html
COPY nginx.conf /etc/nginx/nginx.conf
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]
