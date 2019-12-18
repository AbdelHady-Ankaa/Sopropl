// import {
//   HttpInterceptor,
//   HttpRequest,
//   HttpHandler,
//   HttpEvent,
//   HTTP_INTERCEPTORS,
//   HttpResponse
// } from '@angular/common/http';
// import { Observable } from 'rxjs';
// import { map } from 'rxjs/operators';

// export class JsonInterceptor implements HttpInterceptor {
//   constructor() {}
//   resolveReferences(json) {
//     if (typeof json === 'string') {
//       json = JSON.parse(json);
//     }

//     const byid = {}; // all objects by id
//     const refs = []; // references to objects that could not be resolved
//     json = (function recurse(obj, prop, parent) {
//       if (!(obj instanceof Object) || !obj) {
//         // a primitive value
//         return obj;
//       }
//       if ('$ref' in obj) {
//         // a reference
//         const ref = obj.$ref;
//         if (ref in byid) {
//           return byid[ref];
//         }
//         // else we have to make it lazy:
//         refs.push([parent, prop, ref]);
//         return;
//       } else if ('$id' in obj) {
//         const id = obj.$id;
//         delete obj.$id;
//         if ('$values' in obj) {
//           // an array
//           obj = obj.$values.map(recurse);
//         } else {
//           for (const key in obj) {
//             if (obj.hasOwnProperty(key)) {
//               obj[key] = recurse(obj[key], key, obj);
//             }
//           }
//         }
//         byid[id] = obj;
//       }
//       return obj;
//     })(json); // run it!

//     for (const ref of refs) {
//       // resolve previously unknown references
//       // const ref = refs[i];
//       ref[0][ref[1]] = byid[ref[2]];
//       // Notice that this throws if you put in a reference at top-level
//     }
//     return json;
//   }
//   intercept(
//     req: HttpRequest<any>,
//     next: HttpHandler
//   ): Observable<HttpEvent<any>> {
//     return next.handle(req).pipe(
//       // map(res => {
//       //   if (res instanceof HttpResponse) {
//       //
//       //     const data = this.resolveReferences(res.body);
//       //
//       //   }
//       //   return res;
//       // })
//     );
//   }
// }

// export const JsonInterceprtorProvider = {
//   provide: HTTP_INTERCEPTORS,
//   useClass: JsonInterceptor,
//   multi: true
// };
