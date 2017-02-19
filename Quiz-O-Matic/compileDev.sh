if ls index-dev.html* 1> /dev/null 2>&1;  then
    cp index-dev.html index.html
fi

if ls main-dev.ts* 1> /dev/null 2>&1; then
     cp main-dev.ts main.ts
fi

if ls tsconfig-dev.json* 1> /dev/null 2>&1; then
    cp tsconfig-dev.json tsconfig.json
fi

npm start