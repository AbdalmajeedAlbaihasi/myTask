# واجهة برمجة تطبيقات المنتجات (Products API)

مشروع ASP.NET Core Web API مبني باستخدام معمارية Clean Architecture (Domain, Application, Infrastructure, API) وقاعدة بيانات EF Core In-Memory.

## نقاط النهاية (Endpoints)
* `GET /api/products` - جلب كافة المنتجات
* `GET /api/products/{id}` - جلب منتج محدد
* `POST /api/products` - إضافة منتج جديد
