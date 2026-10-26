# Product Management API — Async/Await & Performance

هذا المشروع تم تطويره كـ مهمة (تاسك) تدريبية لصالح شركة التدريب Huma volve.

مشروع ASP.NET Core Web API احترافي مبني باستخدام معمارية Clean Architecture (Domain, Application, Infrastructure, API) مع تطبيق مفاهيم البرمجة غير المتزامنة (Async/Await)، دعم إلغاء الطلبات (CancellationToken)، وقياس الأداء.

---

## هيكلية المشروع (Architecture Layers)
ينقسم المشروع إلى 4 طبقات رئيسية لضمان فصل المسؤوليات (Separation of Concerns):
1. ProductsDomain: تحتوي على كيانات النظام الأساسية (مثل كيان Product).
2. ProductsApplication: تحتوي على واجهات المستودعات (IProductRepository)، خدمات التطبيق (ProductService)، ومركبات البيانات (DTOs).
3. ProductsInfrastructure: تحتوي على إعدادات قاعدة البيانات (AppDbContext) وتطبيق المستودعات باستخدام Entity Framework Core.
4. Products.Api: تحتوي على وحدات التحكم (Controllers) ونقاط النهاية (Endpoints).

---

## نقاط النهاية (Endpoints)

| العملية | الطريقة (Method) | المسار (Route) | الوصف |
| :--- | :--- | :--- | :--- |
| GetAll | GET | /api/products | جلب كافة المنتجات بشكل غير متزامن |
| GetById | GET | /api/products/{id} | جلب منتج محدد بواسطة المعرف |
| Create | POST | /api/products | إضافة منتج جديد والتحقق من صحة المدخلات |
| Update | PUT | /api/products/{id} | تحديث بيانات منتج موجود |
| Delete | DELETE | /api/products/{id} | حذف منتج من النظام |
| Performance Test | GET | /api/products/performance-test | مقارنة زمن التنفيذ بالتتابع مقابل التوازي باستخدام Stopwatch |

---

## المميزات والإضافات التقنية (Task Requirements)
* Async/Await: استخدام البرمجة غير المتزامنة بالكامل عبر طبقات الـ API، الـ Services، والـ Repositories باستخدام دوال EF Core (ToListAsync, FindAsync, AddAsync, SaveChangesAsync).
* CancellationToken: تمرير الرموز عبر كافة الطبقات للتعامل مع إلغاء الطلبات من قبل المستخدم أو العميل لتحسين استهلاك الموارد.
* Performance Measurement: تطبيق نقطة نهاية قياس الأداء (performance-test) لمقارنة أداء عمليات قواعد البيانات بالتتابع (Sequential) وبالتوازي باستخدام (Task.WhenAll) مع إرفاق تقرير الأداء.
