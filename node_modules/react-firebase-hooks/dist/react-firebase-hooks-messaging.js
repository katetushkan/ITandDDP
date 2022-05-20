!function(e,r,t){"use strict";
/*! *****************************************************************************
    Copyright (c) Microsoft Corporation.

    Permission to use, copy, modify, and/or distribute this software for any
    purpose with or without fee is hereby granted.

    THE SOFTWARE IS PROVIDED "AS IS" AND THE AUTHOR DISCLAIMS ALL WARRANTIES WITH
    REGARD TO THIS SOFTWARE INCLUDING ALL IMPLIED WARRANTIES OF MERCHANTABILITY
    AND FITNESS. IN NO EVENT SHALL THE AUTHOR BE LIABLE FOR ANY SPECIAL, DIRECT,
    INDIRECT, OR CONSEQUENTIAL DAMAGES OR ANY DAMAGES WHATSOEVER RESULTING FROM
    LOSS OF USE, DATA OR PROFITS, WHETHER IN AN ACTION OF CONTRACT, NEGLIGENCE OR
    OTHER TORTIOUS ACTION, ARISING OUT OF OR IN CONNECTION WITH THE USE OR
    PERFORMANCE OF THIS SOFTWARE.
    ***************************************************************************** */var n=function(){return n=Object.assign||function(e){for(var r,t=1,n=arguments.length;t<n;t++)for(var u in r=arguments[t])Object.prototype.hasOwnProperty.call(r,u)&&(e[u]=r[u]);return e},n.apply(this,arguments)},u=function(e){return{loading:null==e,value:e}},o=function(e){var r=e?e():void 0,o=t.useReducer((function(e,r){switch(r.type){case"error":return n(n({},e),{error:r.error,loading:!1,value:void 0});case"reset":return u(r.defaultValue);case"value":return n(n({},e),{error:void 0,loading:!1,value:r.value});default:return e}}),u(r)),a=o[0],i=o[1],l=function(){var r=e?e():void 0;i({type:"reset",defaultValue:r})},s=function(e){i({type:"error",error:e})},c=function(e){i({type:"value",value:e})};return t.useMemo((function(){return{error:a.error,loading:a.loading,reset:l,setError:s,setValue:c,value:a.value}}),[a.error,a.loading,l,s,c,a.value])};e.useToken=function(e,n){var u=o(),a=u.error,i=u.loading,l=u.setError,s=u.setValue,c=u.value;t.useEffect((function(){r.getToken(e,{vapidKey:n}).then(s).catch(l)}),[e]);var v=[c,i,a];return t.useMemo((function(){return v}),v)},Object.defineProperty(e,"__esModule",{value:!0})}(this["react-firebase-hooks"]=this["react-firebase-hooks"]||{},messaging,react);
//# sourceMappingURL=react-firebase-hooks-messaging.js.map
