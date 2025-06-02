# Build stage
FROM node:18-alpine AS build
WORKDIR /app
COPY . .
RUN npm install
RUN npm run build

# Production stage (serve static files)
FROM node:18-alpine AS prod
WORKDIR /app
RUN npm install -g serve
COPY --from=build /app/dist ./dist
EXPOSE 3000
CMD ["serve", "-s", "dist", "-l", "3000"]
