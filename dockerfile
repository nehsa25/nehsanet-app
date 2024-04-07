FROM node:21.6.1-slim
EXPOSE 80 443
WORKDIR /app
COPY package.json .
RUN npm install
COPY . .
CMD node index.js