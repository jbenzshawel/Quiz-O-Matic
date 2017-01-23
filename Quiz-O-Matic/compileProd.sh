if ls index-prod.html* 1> /dev/null 2>&1; then
    cp index-prod.html index.html
fi

if ls main-prod.ts* 1> /dev/null 2>&1; then
    cp main-prod.ts main.ts
fi

if ls tsconfig-aot/json* 1> /dev/null 2>&1; then
    cp tsconfig-aot.json tsconfig.json
fi

node_modules/.bin/ngc -p tsconfig-aot.json && node_modules/.bin/rollup -c rollup-config.js && npm run lite